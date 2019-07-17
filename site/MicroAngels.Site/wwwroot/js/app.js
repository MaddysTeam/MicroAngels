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

function checkCode(errorCode, returnUrl) {
	var code = localStorage.getItem('code');
	if (code === undefined || code === '' || errorCode == 401) {
		location.href = returnUrl;
	}
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
	var $parent = $source.closest('.tab-content')
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
		dataUrl: ''
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
				bAutoWidth: false,
				columnDefs: options.columnsDefs || [],
				columns: columns,
				processing: true,
				aodata: null,
				bProcessing: true,
				bServerSide: true,
				sAjaxSource: options.dataUrl,
				sServerMethod: 'post',
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




