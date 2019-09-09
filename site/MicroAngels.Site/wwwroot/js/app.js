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
var apiDomain = 'http://192.168.1.2:5000';
var local = 'http://localhost:5000';
urls = {
	editUser: '{0}/api/authserver/user/edit'.format(apiDomain),
	editRole: '{0}/api/authserver/role/edit'.format(apiDomain),
	editAssetsList: '{0}/api/authserver/assets/editList'.format(apiDomain),
	editMenu: '{0}/api/authserver/assets/editMenu'.format(apiDomain),
	editInterface: '{0}/api/authserver/assets/editInterface'.format(apiDomain),
	editTopic: '{0}/api/MessageCenter/topic/edit'.format(apiDomain),

	bindRole: '{0}/api/authserver/user/bindRole'.format(apiDomain),
	bindAssets: '{0}/api/authserver/role/bindAssets'.format(apiDomain),
	userEdit: '{0}/api/authserver/user/edit'.format(apiDomain),

	getUsers: '{0}/api/authserver/user/users'.format(apiDomain),
	getRoles: '{0}/api/authserver/role/roles'.format(apiDomain),
	getRolesByUserId: '{0}/api/authserver/role/userRoles'.format(apiDomain),
	getAssets: '{0}/api/authserver/assets/roleAssets'.format(apiDomain),
	getInterface: '{0}/api/authserver/assets/interfaces'.format(apiDomain),
	getMenu: '{0}/api/authserver/assets/allMenus'.format(apiDomain),
	getHierarchyMenus: '{0}/api/authserver/assets/hierarchyMenus'.format(apiDomain),
	getTopics: '{0}/api/messagecenter/topic/topics'.format(apiDomain),
	getAnnounces: '{0}/api/messagecenter/message/announces'.format(apiDomain),
	getUnReadAnnounces: '{0}/api/messagecenter/message/unreadAnnounces'.format(apiDomain),
	getFriends: '{0}/api/messagecenter/subscribe/targets'.format(apiDomain),

	menuInfo: '{0}/api/authserver/assets/menuInfo'.format(apiDomain),
	interfaceInfo: '{0}/api/authserver/assets/interfaceInfo'.format(apiDomain),
	roleInfo: '{0}/api/authserver/role/info'.format(apiDomain),
	userInfo: '{0}/api/authserver/user/info'.format(apiDomain),
	showUser: '{0}/api/authserver/user/briefInfo'.format(apiDomain),
	topicInfo: '{0}/api/messagecenter/topic/info'.format(apiDomain),
	changePassword: '{0}/api/accountservice/changePassword'.format(apiDomain),

	sendAnnounce: '{0}/api/messagecenter/message/sendAnnounce'.format(apiDomain),
	receiveAnnounce: '{0}/api/messagecenter/message/receiveAnnounce'.format(apiDomain),
	subscribe: '{0}/api/messagecenter/subscribe/subscribe'.format(apiDomain),
	unSubscribe: '{0}/api/messagecenter/subscribe/unSubscribe'.format(apiDomain),

	fileUpload: '{0}/api/fileservice/upload'.format(apiDomain),

	login: '{0}/account/login'.format(local),
	index: '{0}/'.format(local),
	signUp: '{0}/api/accountservice/signup'.format(apiDomain),
	signOut: '{0}/api/accountservice/signout'.format(apiDomain),
	userPage: '{0}/user/index'.format(local),
	rolePage: '{0}/role/index'.format(local),
	userEditPage: local + '/user/edit?id={0}',
	roleEditPage: local + '/role/edit?id={0}',
	topicEditPage: local + '/message/topicEdit?id={0}',
	menuEditPage: local + '/assets/EditMenu?menuId={0}',
	interfaceEditPage: local + '/assets/EditInterface?interfaceId={0}',
	bindRolePage: '{0}/user/bindRoles'.format(local),
	interfacePage: '{0}/assets/interfaceIndex'.format(local),
	menuPage: '{0}/assets/menuIndex'.format(local),
	assetsPage: '{0}/assets/index'.format(local),
	announceSendPage: '{0}/message/AnnounceSend'.format(local),
	profilePage: local + '/user/profile?userId={0}',
	changePasswordPage: local + '/account/changePassword?id={0}'
};

topics = {
	friendTopicId: '897fdb63-d5f2-4ff4-a88f-219251a34675'
};

/*
 * global oauth code 
*/
var code = checkCode(urls.login);



function checkCode(returnUrl) {
	var code = localStorage.getItem('code');
	if (code === undefined || code === '' || code == null) {
		location.href = returnUrl;
	}
	return code;
}

function getRefreshCode() {
	return localStorage.getItem('refreshToken');
}

function setToken(token, refreshToken) {
	localStorage.setItem('code', token);
	localStorage.setItem('refreshToken', refreshToken);
}

function cleanCode() {
	localStorage.removeItem('code');
	localStorage.removeItem('refreshToken');
}

function whenForbidden(redirectUrl) {
	cleanCode();
	location.href = redirectUrl;
}

var menuComponents = {
	menuStr: '<li class=""><a href="#" class="dropdown-toggle"><i class="menu-icon fa fa-list"></i><span class="menu-text"> {0} </span><b class="arrow fa fa-angle-down"></b></a></li>',
	submenuStr: '<ul class="submenu"></ul>',
	submenuItemStr: '<li class=""><a href="{1}"><i class="menu-icon fa fa-caret-right"></i>{0}</a><b class="arrow"></b></li>',
};

function showMenu() {
	var code = checkCode(urls.login);
	var ajax = ajaxRequset(urls.getHierarchyMenus, code, urls.login, null, function (data) {
		console.log(data.data);
		if (!data.data) whenForbidden(urls.login);
		showMenuHierarchy($('#SideBar'),data.data);

	});
	$.ajax(ajax);
}

function showUser() {
	var code = checkCode(urls.login);
	var ajax = ajaxRequset(urls.showUser, code, location.href, null, function (data) {
		console.log(data);
		if (!data.data) whenForbidden(urls.login);

		var coverPath = apiDomain + '/api' + data.data.headerImagePath;
		$('.nav-user-photo').attr('src', coverPath);
		$('.user-info').append('<small>{0}</small>'.format(data.data.userName));
		$('.user-profile').attr('href', urls.profilePage.format(data.data.id));
	});

	$.ajax(ajax);
}

function signOut() {
	var code = checkCode(urls.login);
	var ajax = ajaxRequset(urls.signOut, code, location.href, {token:code}, function (data) {
		localStorage.removeItem('code');
		localStorage.removeItem('refreshToken');
		location.href = urls.login;
	});
	$.ajax(ajax);
}

var ajaxRequset = function (url, code, forbiddenUrl, data, success) {
	var ajax = {
		type: "POST",
		url: url,
		xhrFields: {
			withCredentials: false // 设置运行跨域操作
		},
		contentType: 'application/x-www-form-urlencoded',
		dataType: 'json',
		headers: {
			'Authorization': 'Bearer ' + code,
			'AccessToken': code,
			'RefreshToken': getRefreshCode()
		},
		error: function (o) {
			if (!forbiddenUrl)
				forbiddenUrl = urls.login;
			if (o.status == 403 || o.status == 401) {
				whenForbidden(forbiddenUrl);
			}
		},
		success: function (data) {
			if (data.tokens) {
				var token = data.tokens.accessToken;
				var refreshToken = data.tokens.refreshToken;
				console.log('token', token);
				console.log('refresh', refreshToken);

				if (token != '' && refreshToken != '') {
					setToken(token, refreshToken);
				}

				delete data.tokens;
			}
			if (success) {
				success(data);
			}
		}
	};

	if (data) {
		ajax.data = data;
	}

	return ajax;
};

var ajaxRequsetWithoutKey = function (url, code, forbiddenUrl, data, success) {
	var ajax = {
		type: "POST",
		url: url,
		xhrFields: {
			withCredentials: false // 设置运行跨域操作
		},
		contentType: 'application/x-www-form-urlencoded',
		dataType: 'json',
		headers: {
			'Authorization': 'Bearer ' + code,
			'AccessToken': code,
			'RefreshToken': getRefreshCode()
		},
		error: function (o) {
			if (!forbiddenUrl)
				forbiddenUrl = urls.login;
			if (o.status == 403 || o.status == 401) {
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

function showMenuHierarchy($container,data) {
	for (var i = 0; i < data.children.length; i++) {
		if (data.children[i].children.length > 0) {
			var $current = $(menuComponents.menuStr.format(data.children[i].title));
			var $subContainer = $(menuComponents.submenuStr);
			showMenuHierarchy($subContainer,data.children[i]);
			
			$subContainer.appendTo($current);
			$current.appendTo($container);
		}
		else {
			$(menuComponents.submenuItemStr.format(data.children[i].title, data.children[i].link)).appendTo($container);
		}
	}
	return $container;
}

/* ------------------------------------------------- messages------------------------------------------------------- */

function announceNotification() {
	//var code = checkCode(urls.login);
	//setInterval(function () {
	//	var ajax = ajaxRequset(urls.getUnReadAnnounces, code, urls.login, {}, function (data) {
	//		var messageCount = data.data.length;
	//		if (messageCount > 0) {
	//			$('.notification').html('').append('<i class="ace-icon fa fa-bell icon-animated-bell"></i><span class="badge badge-important">{0}</span>'.format(messageCount));
	//			$('.messageCount').html(messageCount);
	//		}
	//		else {
	//			$('.notification').html('').append('<i class="ace-icon fa fa-bell icon-bell"></i>');
	//		}
	//	});

	//	$.ajax(ajax);
	//}, 20000);
}

function reveiveAnnounce() {
	var ajax = ajaxRequset(urls.receiveAnnounce, code, urls.login, {}, function (data) {

		$('.notification').html('').append('<i class="ace-icon fa fa-bell icon-bell"></i>');

	});

	$.ajax(ajax);
}


/* ------------------------------------------------- ajax forms ------------------------------------------------------- */

function ajaxSubmitForm(selector, options) {
	options = $.extend({
		code: null,
		isReplaceCommas: true,
		dataUrl: selector.attr('action'),
		afterSuccess: function () { },
	}, options);


	$.validator.unobtrusive.parse(selector);

	selector.submit(function (e) {
		e.preventDefault();
		var $this = $(this);
		var para = $this.serialize();
		//if (options.isReplaceCommas == true) {
		//	para = para.replace('%2c', ',');
		//}
		console.log(para);
		console.log(options.dataUrl);
		console.log(options.code);
		console.log($this.valid());
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
		directUrlWhenForbidden: ''
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
				ajax: ajaxRequsetWithoutKey(options.dataUrl, options.accessCode, options.forbiddenUrl),
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


function refreshTable(selector) {
	$(selector).DataTable().draw(true);
}


/* ------------------------------------------------- timer ------------------------------------------------------- */

