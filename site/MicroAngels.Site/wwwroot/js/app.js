String.prototype.format = function (args) {
	var result = this;
	if (arguments.length > 0) {
		if (arguments.length == 1 && typeof (args) == "object") {
			for (var key in args) {
				if (args[key] != undefined) {
					var reg = new RegExp("({" + key + "})", "g");
					result = result.replace(reg, args[key]);
				}
			}
		}
		else {
			for (var i = 0; i < arguments.length; i++) {
				if (arguments[i] != undefined) {
					var reg = new RegExp("({[" + i + "]})", "g");
					result = result.replace(reg, arguments[i]);
				}
			}
		}
	}
	return result;
};


/* ------------------------------------------------- bussiness ------------------------------------------------------- */

urls = {
	login: 'http://localhost:5000/account/login',
	index: 'http://localhost:5000/',
	signOut: 'http://192.168.1.2:5000/api/accountservice/signout',

	editUser: 'http://192.168.1.2:5000/api/authserver/user/edit',
	editRole: 'http://192.168.1.2:5000/api/authserver/role/edit',
	editAssetsList: 'http://192.168.1.2:5000/api/authserver/assets/editList',
	editMenu: 'http://192.168.1.2:5000/api/authserver/assets/editMenu',

	bindRole: 'http://192.168.1.2:5000/api/authserver/user/bindRole',
	bindAssets:'http://192.168.1.2:5000/api/authserver/role/bindAssets',
	userEdit: 'http://192.168.1.2:5000/api/authserver/user/edit',

	getUsers: 'http://192.168.1.2:5000/api/authserver/user/users',
	getRoles: 'http://192.168.1.2:5000/api/authserver/role/roles',
	getRolesByUserId: 'http://192.168.1.2:5000/api/authserver/role/userRoles',
	getAssets: 'http://192.168.1.2:5000/api/authserver/assets/roleAssets',
	getInterface: 'http://192.168.1.2:5000/api/authserver/assets/interfaces',
	getMenu: 'http://192.168.1.2:5000/api/authserver/assets/allMenus',

	menuInfo: 'http://192.168.1.2:5000/api/authserver/assets/menuInfo',
	roleInfo: 'http://192.168.1.2:5000/api/authserver/role/info',
	userInfo: 'http://192.168.1.2:5000/api/authserver/user/info',
	showUser: 'http://192.168.1.2:5000/api/authserver/user/briefInfo',

	userPage: 'http://localhost:5000/user/index',
	rolePage: 'http://localhost:5000/role/index',
	userEditPage: 'http://localhost:5000/user/edit?id={0}',
	roleEditPage: 'http://localhost:5000/role/edit?id={0}',
	menuEditPage: 'http://localhost:5000/assets/EditMenu?menuId={0}',
	bindRolePage:'http://localhost:5000/user/bindRoles',
	interfacePage: 'http://localhost:5000/assets/interfaceIndex',
	menuPage:'http://localhost:5000/assets/menuIndex',
	assetsPage: 'http://localhost:5000/assets/index'
};


function checkCode(returnUrl) {
	var code = localStorage.getItem('code');
	if (code === undefined || code === '' || code==null) {
		location.href = returnUrl;
	}
	return code;
}

function cleanCode() {
	localStorage.removeItem('code');
	localStorage.removeItem('refreshCode');
}

function whenForbidden(redirectUrl) {
	cleanCode();
	alert(redirectUrl);
	location.href = redirectUrl;
}

function showMenu() {
	var menuStr = '<li class=""><a href="#" class="dropdown-toggle"><i class="menu-icon fa fa-list"></i><span class="menu-text"> {0} </span><b class="arrow fa fa-angle-down"></b></a></li>';
	var submenuStr = '<ul class="submenu"></ul>'
	var submenuItemStr = '<li class=""><a href="{1}"><i class="menu-icon fa fa-caret-right"></i>{0}</a><b class="arrow"></b></li>';
	var menus = [{
		id: '1', title: '系统设置', children: [
			{
				id: '1.1', title: '用户管理', link: urls.userPage
			},
			{
				id: '1.2', title: '角色管理', link: urls.rolePage
			},
			{
				id: '1.3', title: '资源管理', link: urls.assetsPage
			},
			{
				id: '1.4', title: '接口管理', link: urls.interfacePage
			},
			{
				id: '1.5', title: '菜单管理', link: urls.menuPage
			}
			//{
			//	id: '1.6', title: '按钮管理', link: urls.interfacePage
			//}
		]
	}];


	$(menus).each(function (i) {
		var menu = menus[i];
		var $menu_container = $('#SideBar');
		var $menu_body = $(menuStr.format(menu.title));
		var $submenu_container = $(submenuStr);

		var j = 0;
		while (menu.children) {
			if (j == menu.children.length) break;
			var submenu = menu.children[j++];
			$submenu_container.append(submenuItemStr.format(submenu.title, submenu.link));
		}

		$menu_body.append($submenu_container).appendTo($menu_container)
	});
}

function showUser() {
	var code = checkCode(urls.login);
	var ajax = ajaxRequset(urls.showUser, code, location.href, { userName: 'admin' }, function (data) {
		console.log(data);
		$('.user-info').append('<small>{0}</small>'.format(data.data.userName));
	});

	$.ajax(ajax);
}

function signOut() {
	var code = checkCode(urls.login);
	localStorage.removeItem('code');
	localStorage.removeItem('refreshToken');
	location.href = urls.login;
}


var ajaxRequset = function(url,code,forbiddenUrl,data,success){
	var ajax= {
		type: "POST",
		url: url,
		xhrFields: {
			withCredentials: false // 设置运行跨域操作
		},
		contentType: 'application/x-www-form-urlencoded',
		dataType: 'json',
		headers: {
			'Authorization': 'Bearer ' + code
		},
		error: function (o) {
			//alert(o.status);
			if (o.status == 403) {
				whenForbidden(forbiddenUrl);
			}
		}
	};

	if (data) {
		ajax.data = data;
	}

	if (success) {
		ajax.success = success;
	}

	return ajax;
};

/* ------------------------------------------------- ajax forms ------------------------------------------------------- */

function ajaxSubmitForm(selector, options) {
	options = $.extend({
		code: null,
		isReplaceCommas: true,
		dataUrl: selector.attr('action'),
		afterSuccess: function () {},
	}, options);


	$.validator.unobtrusive.parse(selector);

	selector.submit(function (e) {
		e.preventDefault();
		var $this = $(this);
		var para = $this.serialize();
		if (options.isReplaceCommas == true) {
			para = para.replace('%2c', ',');
		}
		if ($this.valid()) {
			var ajaxReq = ajaxRequset(options.dataUrl, options.code, urls.login, para, options.afterSuccess);
			$.ajax(ajaxReq);
		}
	});
}


/* ------------------------------------------------- ajax modals ------------------------------------------------------- */

$(document).on('click.bs.modal.data-api', '[data-toggle="ajax-modal"]', function (event) {

	var $this = $(this),
		url = $this.data('url'),
		$target = $($this.data('target'));

	$.get(url, function (response) {
		// ajax get form content
		$target
			.html(response)
			.modal({ backdrop: 'static', keyboard: false })
			.find('.form-control').first().focus();
	});

});


/* ------------------------------------------------- ajax dropdown ------------------------------------------------------- */

function ajaxDropdown($selector, options) {
	options = $.extend({
		code: null,
		dataUrl: options.dataUrl,
		para: {} || options.para,
		afterSuccess: function (data) {
			$(data.data).each(function (i) {
				$selector.append("<option value='{0}'>{1}</option>".format(this.id, this.name));
			});
		}
	}, options);

	var ajaxReq = ajaxRequset(options.dataUrl, options.code, urls.login, options.para, options.afterSuccess);
	$.ajax(ajaxReq);

}

/* ------------------------------------------------- menus ------------------------------------------------------- */

$('.sparkline').each(function () {
	var $box = $(this).closest('.infobox');
	var barColor = !$box.hasClass('infobox-dark') ? $box.css('color') : '#FFF';
	$(this).sparkline('html',
		{
			tagValuesAttribute: 'data-values',
			type: 'bar',
			barColor: barColor,
			chartRangeMin: $(this).data('min') || 0
		});
});


$('#recent-box [data-rel="tooltip"]').tooltip({ placement: tooltip_placement });
function tooltip_placement(context, source) {
	var $source = $(source);
	var $parent = $source.closest('.tab-content');
	var off1 = $parent.offset();
	var w1 = $parent.width();

	var off2 = $source.offset();
	//var w2 = $source.width();

	if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
	return 'left';
}


$('.dialogs,.comments').ace_scroll({
	size: 300
});


//Android's default browser somehow is confused when tapping on label which will lead to dragging the task
//so disable dragging when clicking on label
var agent = navigator.userAgent.toLowerCase();
if (ace.vars['touch'] && ace.vars['android']) {
	$('#tasks').on('touchstart', function (e) {
		var li = $(e.target).closest('#tasks li');
		if (li.length == 0) return;
		var label = li.find('label.inline').get(0);
		if (label == e.target || $.contains(label, e.target)) e.stopImmediatePropagation();
	});
}

$('#tasks').sortable({
	opacity: 0.8,
	revert: true,
	forceHelperSize: true,
	placeholder: 'draggable-placeholder',

	forcePlaceholderSize: true,
	tolerance: 'pointer',
	stop: function (event, ui) {
		//just for Chrome!!!! so that dropdowns on items don't appear below other items after being moved
		$(ui.item).css('z-index', 'auto');
	}
}
);
$('#tasks').disableSelection();
$('#tasks input:checkbox').removeAttr('checked').on('click', function () {
	if (this.checked) $(this).closest('li').addClass('selected');
	else $(this).closest('li').removeClass('selected');
});


//show the dropdowns on top or bottom depending on window height and menu position
$('#task-tab .dropdown-hover').on('mouseenter', function (e) {
	var offset = $(this).offset();

	var $w = $(window)
	if (offset.top > $w.scrollTop() + $w.innerHeight() - 100)
		$(this).addClass('dropup');
	else $(this).removeClass('dropup');
});


/* ------------------------------------------------- tables ------------------------------------------------------- */


var initTable = function (id, options) {
	options = $.extend({
		serverSide: false,
		dataUrl: '',
		accessCode: '',
		directUrlWhenForbidden:''
	}, options);

	var $tableSelector = $('#' + id);
	var columns = [];

	$tableSelector.find('tr > th').each(function (i) {
		var $this = $(this);
		if ($this.data('operation') != 'command') {
			columns[i] = {
				'data': $this.data('column')
			};
		}
	});

	var myTable =
		$tableSelector
			//.wrap("<div class='dataTables_borderWrap' />")   //if you are applying horizontal scrolling (sScrollX)
			.DataTable({
				ajax: ajaxRequset(options.dataUrl,options.accessCode, options.forbiddenUrl),
				bAutoWidth: false,
				columnDefs: options.columnsDefs || [],
				columns: columns,
				processing: true,
				aodata: null,
				bProcessing: true,
				bServerSide: true,
				bPaginate: true,
				iDisplayLength: 10,
				language: {
					"sProcessing": "处理中...",
					"sLengthMenu": "显示 _MENU_ 项结果",
					"sZeroRecords": "没有匹配结果",
					"sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
					"sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
					"sInfoFiltered": "(由 _MAX_ 项结果过滤)",
					"sInfoPostFix": "",
					"sSearch": "搜索:",
					"sUrl": "",
					"sEmptyTable": "表中数据为空",
					"sLoadingRecords": "载入中...",
					"sInfoThousands": ",",
					"oPaginate": {
						"sFirst": "首页",
						"sPrevious": "上页",
						"sNext": "下页",
						"sLast": "末页"
					},
					"oAria": {
						"sSortAscending": ": 以升序排列此列",
						"sSortDescending": ": 以降序排列此列"
					}
				}
				//serverSide: options.serverSide,
				//"aoColumns": [
				//	{ "bSortable": false },
				//	null, null, null, null,
				//	{ "bSortable": false }
				//],
				//"sScrollY": "200px",
				//"sScrollX": "100%",
				//"sScrollXInner": "120%",
				//"bScrollCollapse": true,
				//Note: if you are applying horizontal scrolling (sScrollX) on a ".table-bordered"
				//you may want to wrap the table inside a "div.dataTables_borderWrap" element
				//select: {
				//	style: 'multi'
				//}
			});

	//initiate dataTables plugin
	$.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap';

	new $.fn.dataTable.Buttons(myTable, {
		buttons: [
			{
				"extend": "colvis",
				"text": "<i class='fa fa-search bigger-110 blue'></i> <span class='hidden'>Show/hide columns</span>",
				"className": "btn btn-white btn-primary btn-bold",
				columns: ':not(:first):not(:last)'
			},
			{
				"extend": "copy",
				"text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
				"className": "btn btn-white btn-primary btn-bold"
			},
			{
				"extend": "csv",
				"text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
				"className": "btn btn-white btn-primary btn-bold"
			},
			{
				"extend": "excel",
				"text": "<i class='fa fa-file-excel-o bigger-110 green'></i> <span class='hidden'>Export to Excel</span>",
				"className": "btn btn-white btn-primary btn-bold"
			},
			{
				"extend": "pdf",
				"text": "<i class='fa fa-file-pdf-o bigger-110 red'></i> <span class='hidden'>Export to PDF</span>",
				"className": "btn btn-white btn-primary btn-bold"
			},
			{
				"extend": "print",
				"text": "<i class='fa fa-print bigger-110 grey'></i> <span class='hidden'>Print</span>",
				"className": "btn btn-white btn-primary btn-bold",
				autoPrint: false,
				message: 'This print was produced using the Print button for DataTables'
			}
		]
	});

	myTable.buttons().container().appendTo($('.tableTools-container'));


	//style the message box
	var defaultCopyAction = myTable.button(1).action();
	myTable.button(1).action(function (e, dt, button, config) {
		defaultCopyAction(e, dt, button, config);
		$('.dt-button-info').addClass('gritter-item-wrapper gritter-info gritter-center white');
	});


	var defaultColvisAction = myTable.button(0).action();
	myTable.button(0).action(function (e, dt, button, config) {

		defaultColvisAction(e, dt, button, config);


		if ($('.dt-button-collection > .dropdown-menu').length == 0) {
			$('.dt-button-collection')
				.wrapInner('<ul class="dropdown-menu dropdown-light dropdown-caret dropdown-caret" />')
				.find('a').attr('href', '#').wrap("<li />")
		}
		$('.dt-button-collection').appendTo('.tableTools-container .dt-buttons')
	});

	////

	setTimeout(function () {
		$($('.tableTools-container')).find('a.dt-button').each(function () {
			var div = $(this).find(' > div').first();
			if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
			else $(this).tooltip({ container: 'body', title: $(this).text() });
		});
	}, 500);

	myTable.on('select', function (e, dt, type, index) {
		if (type === 'row') {
			$(myTable.row(index).node()).find('input:checkbox').prop('checked', true);
		}
	});
	myTable.on('deselect', function (e, dt, type, index) {
		if (type === 'row') {
			$(myTable.row(index).node()).find('input:checkbox').prop('checked', false);
		}
	});

	/////////////////////////////////
	//table checkboxes
	$('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

	//select/deselect all rows according to table header checkbox
	$('#dynamic-table > thead > tr > th input[type=checkbox], #dynamic-table_wrapper input[type=checkbox]').eq(0).on('click', function () {
		var th_checked = this.checked;//checkbox inside "TH" table header

		$('#dynamic-table').find('tbody > tr').each(function () {
			var row = this;
			if (th_checked) myTable.row(row).select();
			else myTable.row(row).deselect();
		});
	});

	//select/deselect a row when the checkbox is checked/unchecked
	$('#dynamic-table').on('click', 'td input[type=checkbox]', function () {
		var row = $(this).closest('tr').get(0);
		if (this.checked) myTable.row(row).deselect();
		else myTable.row(row).select();
	});



	$(document).on('click', '#dynamic-table .dropdown-toggle', function (e) {
		e.stopImmediatePropagation();
		e.stopPropagation();
		e.preventDefault();
	});



	//And for the first simple table, which doesn't have TableTools or dataTables
	//select/deselect all rows according to table header checkbox
	var active_class = 'active';
	$('#simple-table > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
		var th_checked = this.checked;//checkbox inside "TH" table header

		$(this).closest('table').find('tbody > tr').each(function () {
			var row = this;
			if (th_checked) $(row).addClass(active_class).find('input[type=checkbox]').eq(0).prop('checked', true);
			else $(row).removeClass(active_class).find('input[type=checkbox]').eq(0).prop('checked', false);
		});
	});

	//select/deselect a row when the checkbox is checked/unchecked
	$('#simple-table').on('click', 'td input[type=checkbox]', function () {
		var $row = $(this).closest('tr');
		if ($row.is('.detail-row ')) return;
		if (this.checked) $row.addClass(active_class);
		else $row.removeClass(active_class);
	});

	/********************************/
	//add tooltip for small view action buttons in dropdown menu
	$('[data-rel="tooltip"]').tooltip({ placement: tooltip_placement });

	//tooltip placement on right or left
	function tooltip_placement(context, source) {
		var $source = $(source);
		var $parent = $source.closest('table');
		var off1 = $parent.offset();
		var w1 = $parent.width();

		var off2 = $source.offset();
		//var w2 = $source.width();

		if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
		return 'left';
	}


	/***************/
	$('.show-details-btn').on('click', function (e) {
		e.preventDefault();
		$(this).closest('tr').next().toggleClass('open');
		$(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
	});
	/***************/

	/**
	add horizontal scrollbars to a simple table
	$('#simple-table').css({'width':'2000px', 'max-width': 'none'}).wrap('<div style="width: 1000px;" />').parent().ace_scroll(
	  {
		horizontal: true,
		styleClass: 'scroll-top scroll-dark scroll-visible',//show the scrollbars on top(default is bottom)
		size: 2000,
		mouseWheelLock: true
	  }
	).css('padding-top', '12px');
	*/

	return myTable;
};




