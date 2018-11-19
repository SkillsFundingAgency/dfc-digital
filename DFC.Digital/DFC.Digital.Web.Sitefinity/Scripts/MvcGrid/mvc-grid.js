/*!
 * Mvc.Grid 5.0.0
 * https://github.com/NonFactors/MVC5.Grid
 *
 * Copyright © NonFactors
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
var MvcGrid = (function () {
    function MvcGrid(grid, options) {
        this.columns = [];
        this.element = grid;
        options = options || {};
        this.data = options.data;
        this.name = grid.attr('data-name');
        this.rowClicked = options.rowClicked;
        this.methods = { reload: this.reload };
        this.reloadEnded = options.reloadEnded;
        this.loadingDelay = options.loadingDelay;
        this.reloadFailed = options.reloadFailed;
        this.reloadStarted = options.reloadStarted;
        this.requestType = options.requestType || 'get';
        this.prefix = this.name == '' ? '' : this.name + '-';
        this.filterMode = grid.attr('data-filter-mode') || 'ExcelRow';
        this.showLoading = options.showLoading == null || options.showLoading;
        this.sourceUrl = options.sourceUrl || grid.attr('data-source-url') || '';
        this.filters = $.extend({
            'enum': new MvcGridEnumFilter(),
            'date': new MvcGridDateFilter(),
            'text': new MvcGridTextFilter(),
            'number': new MvcGridNumberFilter(),
            'boolean': new MvcGridBooleanFilter()
        }, options.filters);

        if (this.sourceUrl) {
            var splitIndex = this.sourceUrl.indexOf('?');
            if (splitIndex > -1) {
                this.query = this.sourceUrl.substring(splitIndex + 1);
                this.sourceUrl = this.sourceUrl.substring(0, splitIndex);
            } else {
                this.query = options.query == null ? '' : options.query;
            }
        } else {
            this.query = window.location.search.replace('?', '');
        }

        var isLoaded = !this.sourceUrl || (options.isLoaded == null ? grid.children().length > 0 : options.isLoaded);
        if (options.reload || !isLoaded) {
            this.reload();
            return;
        }

        var headers = grid.find('.mvc-grid-headers th');
        var rowFilters = grid.find('.mvc-grid-row-filters th');

        for (var i = 0; i < headers.length; i++) {
            var column = this.createColumn($(headers[i]), rowFilters[i]);

            this.bindFilter(column);
            this.bindSort(column);
            this.cleanup(column);

            this.columns.push(column);
        }

        var pager = grid.find('.mvc-grid-pager');
        if (pager.length > 0) {
            this.pager = {
                currentPage: pager.find('li.active a').attr('data-page') || 0,
                showPageSizes: pager.attr('data-show-page-sizes') == 'True',
                rowsPerPage: pager.find('.mvc-grid-pager-rows'),
                pages: pager.find('li:not(.disabled)'),
                element: pager
            };
        }

        this.bindPager();
        this.bindGrid();
        this.clean();
    }

    MvcGrid.prototype = {
        createColumn: function (header, rowFilter) {
            var column = {};
            column.header = header;
            column.rowFilter = rowFilter;
            column.name = header.attr('data-name');

            if (header.attr('data-filter') == 'True') {
                var options = header.find('.mvc-grid-options').remove();

                column.filter = {
                    isMulti: header.attr('data-filter-multi') == 'True',
                    operator: header.attr('data-filter-operator'),
                    hasOptions: options.children().length > 0,
                    name: header.attr('data-filter-name'),
                    first: {
                        method: header.attr('data-filter-first-method'),
                        value: header.attr('data-filter-first-value'),
                        options: options
                    },
                    second: {
                        method: header.attr('data-filter-second-method'),
                        value: header.attr('data-filter-second-value'),
                        options : options
                    }
                };
            }

            if (header.attr('data-sort') == 'True' && this.filterMode != 'HeaderRow') {
                column.sort = {
                    firstOrder: header.attr('data-sort-first'),
                    order: header.attr('data-sort-order')
                }
            }

            return column;
        },
        set: function (options) {
            for (var key in options) {
                if (this.hasOwnProperty(key)) {
                    if (key == 'filters') {
                        this.filters = $.extend(this.filters, options.filters);
                    } else if (key == 'sourceUrl') {
                        if (!options.hasOwnProperty('query')) {
                            this.query = '';
                        }

                        this.sourceUrl = options.sourceUrl;
                    } else {
                        this[key] = options[key];
                    }
                }
            }

            if (options.reload) {
                this.reload();
            }
        },

        bindFilter: function (column) {
            if (column.filter && this.filters[column.filter.name]) {
                var grid = this;
                var filter = this.filters[column.filter.name];
                var popup = $('body').children('.mvc-grid-popup');

                $(column.rowFilter || column.header[0]).find('.mvc-grid-filter').on('click.mvcgrid', function (e) {
                    e.preventDefault();

                    grid.renderFilter(column, filter);
                });

                $(column.rowFilter).find('.mvc-grid-value').on('keyup.mvcgrid', function (e) {
                    if (filter.isValid(this.value)) {
                        $(this).removeClass('invalid');

                        if (e.which == 13) {
                            filter.apply(grid, column, popup);
                        }
                    } else {
                        $(this).addClass('invalid');
                    }
                });

                var method = $(column.rowFilter).find('.mvc-grid-method');
                if (method.val() == '') {
                    method.val(filter.methods[0]);
                }

                var input = $(column.rowFilter).find('.mvc-grid-value');
                if (input.length && !filter.isValid(input.val())) {
                    input.addClass('invalid');
                }

                filter.initRowFilter(grid, column, popup);
            }
        },
        bindSort: function (column) {
            if (column.sort) {
                var grid = this;

                column.header.on('click.mvcgrid', function (e) {
                    var target = $(e.target || e.srcElement);
                    if (!target.hasClass('mvc-grid-filter') && target.parents('.mvc-grid-filter').length == 0) {
                        grid.applySort(column);
                        grid.reload();
                    }
                });
            }
        },
        bindPager: function () {
            var grid = this;

            if (grid.pager) {
                grid.pager.rowsPerPage.on('change.mvcgrid', function () {
                    grid.applyPage(grid.pager.currentPage);
                    grid.reload();
                });

                grid.pager.pages.on('click.mvcgrid', 'a', function (e) {
                    e.preventDefault();

                    var page = $(this).attr('data-page');

                    if (page) {
                        grid.applyPage(page);
                        grid.reload();
                    }
                });
            }
        },
        bindGrid: function () {
            var grid = this;

            grid.element.find('tbody tr:not(.mvc-grid-empty-row)').on('click.mvcgrid', function (e) {
                var cells = $(this).find('td');
                var data = [];

                for (var i = 0; i < grid.columns.length; i++) {
                    var column = grid.columns[i];
                    if (i < cells.length) {
                        data[column.name] = $(cells[i]).text();
                    }
                }

                if (grid.rowClicked) {
                    grid.rowClicked(this, data, e);
                }

                $(this).trigger('rowclick', [data, grid, e]);
            });
        },

        reload: function () {
            var grid = this;

            grid.element.trigger('reloadStarted', [grid]);

            if (grid.reloadStarted) {
                grid.reloadStarted();
            }

            if (grid.sourceUrl) {
                grid.startLoading();

                $.ajax({
                    cache: false,
                    data: grid.data,
                    type: grid.requestType,
                    url: grid.sourceUrl + '?' + grid.query
                }).done(function (result) {
                    var newGridHtml = $(result);
                    grid.element.replaceWith(newGridHtml);

                    var newGrid = newGridHtml.mvcgrid({
                        reloadStarted: grid.reloadStarted,
                        reloadFailed: grid.reloadFailed,
                        loadingDelay: grid.loadingDelay,
                        reloadEnded: grid.reloadEnded,
                        showLoading: grid.showLoading,
                        requestType: grid.requestType,
                        rowClicked: grid.rowClicked,
                        sourceUrl: grid.sourceUrl,
                        filters: grid.filters,
                        query: grid.query,
                        data: grid.data,
                        isLoaded: true
                    }).data('mvc-grid');

                    newGrid.element.trigger('reloadEnded', [newGrid]);

                    if (newGrid.reloadEnded) {
                        newGrid.reloadEnded();
                    }
                }).fail(function (result) {
                    grid.element.trigger('reloadFailed', [grid, result]);

                    if (grid.reloadFailed) {
                        grid.reloadFailed(result);
                    }
                });
            } else {
                window.location.href = '?' + grid.query;
            }
        },
        renderFilter: function (column, filter) {
            var grid = this;
            var popup = $('body').children('.mvc-grid-popup');

            $(window).off('resize.mvcgrid');
            $(window).off('click.mvcgrid');

            filter.render(grid, popup, column.filter);
            filter.init(grid, column, popup);

            grid.setFilterPosition(column, popup);
            popup.addClass('open');

            $(window).on('click.mvcgrid', function (e) {
                var target = $(e.target || e.srcElement);
                if (!target.hasClass('mvc-grid-filter') && target.parents('.mvc-grid-popup').length == 0 &&
                    !target.is('[class*="ui-datepicker"]') && target.parents('[class*="ui-datepicker"]').length == 0) {
                    $(window).off('click.mvcgrid');
                    popup.removeClass('open');
                }
            });

            $(window).on('resize.mvcgrid', function () {
                if (popup.hasClass('open')) {
                    popup.removeClass('open');

                    grid.setFilterPosition(column, popup);

                    popup.addClass('open');
                }
            });
        },
        setFilterPosition: function (column, popup) {
            var filter = $(column.rowFilter || column.header[0]).find('.mvc-grid-filter');
            var documentWidth = $(document).width();
            var arrow = popup.find('.popup-arrow');
            var popupWidth = popup.width();

            var popupTop = filter.offset().top + filter.height() / 2 + 14;
            var popupLeft = filter.offset().left - 8;
            var arrowLeft = filter.width() / 2;

            if (popupLeft + popupWidth + 13 > documentWidth) {
                var overflow = popupLeft - (documentWidth - popupWidth - 5);
                popupLeft -= overflow;
                arrowLeft += overflow;
            }

            arrow.css('left', arrowLeft + 'px');
            popup.css('left', popupLeft + 'px');
            popup.css('top', popupTop + 'px');
        },
        startLoading: function () {
            if (!this.showLoading || this.element.children('.mvc-grid-loader').length) {
                return;
            }

            var loader = $('<div class="mvc-grid-loader">' +
                '<div>' +
                    '<div></div>' +
                    '<div></div>' +
                    '<div></div>' +
                '</div>' +
            '</div>');

            setTimeout(function () {
                loader.addClass('mvc-grid-loading');
            }, this.loadingDelay == null ? 300 : this.loadingDelay);

            this.element.append(loader);
        },

        cancelFilter: function (column) {
            this.queryRemove(this.prefix + 'page');
            this.queryRemove(this.prefix + 'rows');
            this.queryRemoveStartingWith(this.prefix + column.name + '-');
        },
        applyFilter: function (column) {
            this.cancelFilter(column);

            this.queryAdd(this.prefix + column.name + '-' + column.filter.first.method, column.filter.first.value);
            if (this.filterMode == 'ExcelRow' && column.filter.isMulti) {
                this.queryAdd(this.prefix + column.name + '-op', column.filter.operator);
                this.queryAdd(this.prefix + column.name + '-' + column.filter.second.method, column.filter.second.value);
            }

            if (this.pager && this.pager.showPageSizes) {
                this.queryAdd(this.prefix + 'rows', this.pager.rowsPerPage.val());
            }
        },
        applySort: function (column) {
            this.queryRemove(this.prefix + 'sort');
            this.queryRemove(this.prefix + 'order');
            this.queryAdd(this.prefix + 'sort', column.name);
            var order = column.sort.order == 'asc' ? 'desc' : 'asc';
            if (!column.sort.order && column.sort.firstOrder) {
                order = column.sort.firstOrder;
            }

            this.queryAdd(this.prefix + 'order', order);
        },
        applyPage: function (page) {
            this.queryRemove(this.prefix + 'page');
            this.queryRemove(this.prefix + 'rows');

            this.queryAdd(this.prefix + 'page', page);

            if (this.pager.showPageSizes) {
                this.queryAdd(this.prefix + 'rows', this.pager.rowsPerPage.val());
            }
        },

        queryAdd: function (key, value) {
            this.query += (this.query ? '&' : '') + encodeURIComponent(key) + '=' + encodeURIComponent(value == null ? '' : value);
        },
        queryRemoveStartingWith: function (key) {
            var keyToRemove = encodeURIComponent(key);
            var params = this.query.split('&');
            var newParams = [];

            for (var i = 0; i < params.length; i++) {
                var paramKey = params[i].split('=')[0];
                if (params[i] && paramKey.indexOf(keyToRemove) != 0) {
                    newParams.push(params[i]);
                }
            }

            this.query = newParams.join('&');
        },
        queryRemove: function (key) {
            var keyToRemove = encodeURIComponent(key);
            var params = this.query.split('&');
            var newParams = [];

            for (var i = 0; i < params.length; i++) {
                var paramKey = params[i].split('=')[0];
                if (params[i] && paramKey != keyToRemove) {
                    newParams.push(params[i]);
                }
            }

            this.query = newParams.join('&');
        },

        cleanup: function (column) {
            var header = column.header;
            header.removeAttr('data-name');

            header.removeAttr('data-filter');
            header.removeAttr('data-filter-name');
            header.removeAttr('data-filter-multi');
            header.removeAttr('data-filter-operator');
            header.removeAttr('data-filter-first-value');
            header.removeAttr('data-filter-first-method');
            header.removeAttr('data-filter-second-value');
            header.removeAttr('data-filter-second-method');

            header.removeAttr('data-sort');
            header.removeAttr('data-sort-order');
            header.removeAttr('data-sort-first');
        },
        clean: function () {
            this.element.removeAttr('data-filter-mode');
            this.element.removeAttr('data-source-url');

            if (this.pager) {
                this.pager.element.removeAttr('data-show-page-sizes');
            }
        }
    };

    return MvcGrid;
})();

function MvcGridExtends(subclass, base) {
    Object.setPrototypeOf(subclass, base);

    function Subclass() {
        this.constructor = subclass;
    }

    subclass.prototype = (Subclass.prototype = base.prototype, new Subclass());
}

var MvcGridFilter = (function () {
    function MvcGridFilter() {
        this.methods = [];
    }

    MvcGridFilter.prototype = {
        render: function (grid, popup, filter) {
            popup.html(
                '<div class="popup-arrow"></div>' +
                '<div class="popup-content">' +
                    '<div class="first-filter">' +
                        this.renderFilter(grid, filter.first) +
                    '</div>' +
                    (grid.filterMode == 'ExcelRow' && filter.isMulti ?
                    this.renderOperator(grid, filter, $.fn.mvcgrid.lang.operator) +
                    '<div class="second-filter">' +
                        this.renderFilter(grid, filter.second) +
                    '</div>'
                    : '') +
                    this.renderActions(grid, $.fn.mvcgrid.lang.filter) +
                '</div>');

            popup.find('.mvc-grid-operator').val(filter.operator);
            popup.find('.first-filter .mvc-grid-method').val(filter.first.method || this.methods[0]);
            popup.find('.first-filter .mvc-grid-value').val(filter.first.value);
            popup.find('.second-filter .mvc-grid-method').val(filter.second.method || this.methods[0]);
            popup.find('.second-filter .mvc-grid-value').val(filter.second.value);
        },
        renderFilter: function (grid, filter, lang) {
            var hasOptions = filter.options.children().length;
            var methods = this.methods.reduce(function (all, method) {
                return all + '<option value="' + method + '">' + lang[method] + '</option>';
            }, '');

            return '<div class="popup-group">' +
                       '<select class="mvc-grid-method">' +
                            methods +
                       '</select>' +
                   '</div>' +
                   (grid.filterMode == 'ExcelRow' || (grid.filterMode == 'HeaderRow' && hasOptions) ?
                   '<div class="popup-group">' +
                   (hasOptions ?
                       '<select class="mvc-grid-value">' + filter.options.html() + '</select>' :
                       '<input class="mvc-grid-value" />') +
                   '</div>'
                   :
                   '');
        },
        renderOperator: function (grid, filter, lang) {
            return '<div class="operator-filter">' +
                       '<div class="popup-group">' +
                           '<select class="mvc-grid-operator">' +
                               '<option value="">' + lang.select + '</option>' +
                               '<option value="and">' + lang.and + '</option>' +
                               '<option value="or">' + lang.or + '</option>' +
                           '</select>' +
                       '</div>' +
                   '</div>';
        },
        renderActions: function (grid, lang) {
            return '<div class="filter-actions">' +
                       '<button class="mvc-grid-apply" type="button">' + lang.apply + '</button>' +
                       '<button class="mvc-grid-cancel" type="button">' + lang.remove + '</button>' +
                   '</div>';
        },

        init: function (grid, column, popup) {
            this.bindMethod(grid, column, popup);
            this.bindValue(grid, column, popup);
            this.bindApply(grid, column, popup);
            this.bindCancel(grid, column, popup);
        },
        initRowFilter: function (grid, column, popup) {
            var filter = this;

            $(column.rowFilter).on('change.mvcgrid', 'select.mvc-grid-value', function () {
                filter.apply(grid, column, popup);
            });

            if (grid.filterMode == 'HeaderRow' && column.filter.hasOptions) {
                $(column.rowFilter).find('input.mvc-grid-value').attr('readonly', 'readonly');
            }
        },

        bindMethod: function (grid, column, popup) {
            popup.find('.mvc-grid-method').on('change.mvcgrid', function () {
                $(column.rowFilter).find('.mvc-grid-method').val(this.value);
            });
        },
        bindValue: function (grid, column, popup) {
            var filter = this;

            var inputs = popup.find('.mvc-grid-value');
            inputs.on('keyup.mvcgrid', function (e) {
                if (filter.isValid(this.value)) {
                    $(this).removeClass('invalid');

                    if (e.which == 13) {
                        filter.apply(grid, column, popup);
                    }
                } else {
                    $(this).addClass('invalid');
                }
            });

            for (var i = 0; i < inputs.length; i++) {
                if (!filter.isValid(inputs[i].value)) {
                    $(inputs[i]).addClass('invalid');
                }
            }
        },
        bindApply: function (grid, column, popup) {
            var filter = this;

            popup.find('.mvc-grid-apply').on('click.mvcgrid', function () {
                filter.apply(grid, column, popup);
            });
        },
        bindCancel: function (grid, column, popup) {
            var filter = this;

            popup.find('.mvc-grid-cancel').on('click.mvcgrid', function () {
                filter.cancel(grid, column, popup);
            });
        },

        isValid: function (value) {
            return true;
        },
        apply: function (grid, column, popup) {
            popup.removeClass('open');

            var container = $(column.rowFilter || popup);

            column.filter.operator = container.find('.mvc-grid-operator').val();
            column.filter.first.method = container.find('.first-filter .mvc-grid-method').val();
            column.filter.second.method = container.find('.second-filter .mvc-grid-method').val();

            if (grid.filterMode != 'FilterRow' && column.filter.hasOptions) {
                column.filter.first.value = popup.find('.first-filter .mvc-grid-value').val();
                column.filter.second.value = popup.find('.second-filter .mvc-grid-value').val();
            } else {
                column.filter.first.value = container.find('.first-filter .mvc-grid-value').val();
                column.filter.second.value = container.find('.second-filter .mvc-grid-value').val();
            }

            grid.applyFilter(column);
            grid.reload();
        },
        cancel: function (grid, column, popup) {
            popup.removeClass('open');

            if (column.filter.first.method || column.filter.second.method) {
                grid.cancelFilter(column);
                grid.reload();
            }
        }
    };

    return MvcGridFilter;
})();

var MvcGridTextFilter = (function (base) {
    MvcGridExtends(MvcGridTextFilter, base);

    function MvcGridTextFilter() {
        base.apply(this);

        this.methods = ['contains', 'equals', 'not-equals', 'starts-with', 'ends-with'];
    }

    MvcGridTextFilter.prototype.renderFilter = function (grid, filter) {
        return base.prototype.renderFilter.call(this, grid, filter, $.fn.mvcgrid.lang.text);
    };

    return MvcGridTextFilter;
})(MvcGridFilter);

var MvcGridNumberFilter = (function (base) {
    MvcGridExtends(MvcGridNumberFilter, base);

    function MvcGridNumberFilter() {
        base.apply(this);

        this.methods = ['equals', 'not-equals', 'less-than', 'greater-than', 'less-than-or-equal', 'greater-than-or-equal'];
    }

    MvcGridNumberFilter.prototype.renderFilter = function (grid, filter) {
        return base.prototype.renderFilter.call(this, grid, filter, $.fn.mvcgrid.lang.number);
    };

    MvcGridNumberFilter.prototype.isValid = function (value) {
        return !value || /^(?=.*\d+.*)[-+]?\d*[.,]?\d*$/.test(value);
    };

    return MvcGridNumberFilter;
})(MvcGridFilter);

var MvcGridDateFilter = (function (base) {
    MvcGridExtends(MvcGridDateFilter, base);

    function MvcGridDateFilter() {
        base.apply(this);

        this.methods = ['equals', 'not-equals', 'earlier-than', 'later-than', 'earlier-than-or-equal', 'later-than-or-equal'];
    }

    MvcGridDateFilter.prototype.renderFilter = function (grid, filter) {
        return base.prototype.renderFilter.call(this, grid, filter, $.fn.mvcgrid.lang.date);
    };

    MvcGridDateFilter.prototype.initRowFilter = function (grid, column, popup) {
        var filter = this;

        if ($.fn.datepicker) {
            $(column.rowFilter).find('.mvc-grid-value').datepicker({
                onSelect: function (value, data) {
                    if (value != data.lastVal) {
                        filter.apply(grid, column, popup);
                    }
                }
            });
        }
    };

    MvcGridDateFilter.prototype.bindValue = function (grid, column, popup) {
        if ($.fn.datepicker) {
            popup.find('.mvc-grid-value').datepicker();
        }

        base.prototype.bindValue.call(this, grid, column, popup);
    };

    return MvcGridDateFilter;
})(MvcGridFilter);

var MvcGridEnumFilter = (function (base) {
    MvcGridExtends(MvcGridEnumFilter, base);

    function MvcGridEnumFilter() {
        base.apply(this);

        this.methods = ['equals', 'not-equals'];
    }

    MvcGridEnumFilter.prototype.renderFilter = function (grid, filter) {
        return base.prototype.renderFilter.call(this, grid, filter, $.fn.mvcgrid.lang.enum);
    };

    return MvcGridEnumFilter;
})(MvcGridFilter);

var MvcGridBooleanFilter = (function (base) {
    MvcGridExtends(MvcGridBooleanFilter, base);

    function MvcGridBooleanFilter() {
        base.apply(this);

        this.methods = ['equals', 'not-equals'];
    }

    MvcGridBooleanFilter.prototype.renderFilter = function (grid, filter) {
        return base.prototype.renderFilter.call(this, grid, filter, $.fn.mvcgrid.lang.boolean);
    };

    return MvcGridBooleanFilter;
})(MvcGridFilter);

$.fn.mvcgrid = function (options) {
    var args = arguments;

    if (options === 'instance') {
        var instances = [];

        for (var i = 0; i < this.length; i++) {
            var grid = $(this[i]).closest('.mvc-grid');
            if (!grid.length)
                continue;

            var instance = grid.data('mvc-grid');

            if (!instance) {
                grid.data('mvc-grid', instance = new MvcGrid(grid, options));
            }

            instances.push(instance);
        }

        return this.length <= 1 ? instances[0] : instances;
    }

    return this.each(function () {
        var grid = $(this).closest('.mvc-grid');
        if (!grid.length)
            return;

        var instance = grid.data('mvc-grid');

        if (!instance) {
            if (typeof options == 'string') {
                instance = new MvcGrid(grid);
                instance.methods[options].apply(instance, [].slice.call(args, 1));
            } else {
                instance = new MvcGrid(grid, options);
            }

            $.data(grid[0], 'mvc-grid', instance);
        } else if (typeof options == 'string') {
            instance.methods[options].apply(instance, [].slice.call(args, 1));
        } else if (options) {
            instance.set(options);
        }
    });
};

$.fn.mvcgrid.lang = {
    text: {
        'contains': 'Contains',
        'equals': 'Equals',
        'not-equals': 'Not equals',
        'starts-with': 'Starts with',
        'ends-with': 'Ends with'
    },
    number: {
        'equals': 'Equals',
        'not-equals': 'Not equals',
        'less-than': 'Less than',
        'greater-than': 'Greater than',
        'less-than-or-equal': 'Less than or equal',
        'greater-than-or-equal': 'Greater than or equal'
    },
    date: {
        'equals': 'Equals',
        'not-equals': 'Not equals',
        'earlier-than': 'Earlier than',
        'later-than': 'Later than',
        'earlier-than-or-equal': 'Earlier than or equal',
        'later-than-or-equal': 'Later than or equal'
    },
    enum: {
        'equals': 'Equals',
        'not-equals': 'Not equals'
    },
    boolean: {
        'equals': 'Equals',
        'not-equals': 'Not equals'
    },
    filter: {
        'apply': '&#10004;',
        'remove': '&#10008;'
    },
    operator: {
        'select': '',
        'and': 'and',
        'or': 'or'
    }
};

$(function () {
    $('body').append('<div class="mvc-grid-popup"></div>');
});
