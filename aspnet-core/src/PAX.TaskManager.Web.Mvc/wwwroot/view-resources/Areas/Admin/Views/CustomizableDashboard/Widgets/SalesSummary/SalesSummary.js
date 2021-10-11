(function () {
    app.widgets.Widgets_Tenant_SalesSummary = function () {
        var _tenantDashboardService = abp.services.app.tenantDashboard;
        var _widget;

        var salesSummaryDatePeriod = {
            daily: 1,
            weekly: 2,
            monthly: 3
        };

        this.init = function (widgetManager) {
            _widget = widgetManager.getWidget();
            getSalesSummary();
        };

        var initSalesSummaryChart = function (salesSummaryData, totalSales, revenue, expenses, growth) {
            //Sales summary => MorrisJs: https://github.com/morrisjs/morris.js/

            var SalesSummaryChart = function (element) {
                var instance = null;

                var init = function (data) {
                    return new Morris.Area({
                        element: element,
                        padding: 0,
                        behaveLikeLine: false,
                        gridEnabled: false,
                        gridLineColor: false,
                        axes: false,
                        fillOpacity: 1,
                        data: data,
                        lineColors: ['#399a8c', '#92e9dc'],
                        xkey: 'period',
                        ykeys: ['sales', 'profit'],
                        labels: ['Sales', 'Profit'],
                        pointSize: 0,
                        lineWidth: 0,
                        hideHover: 'auto',
                        resize: true
                    });
                };

                var refresh = function (datePeriod) {
                    var self = this;
                    _tenantDashboardService
                        .getSalesSummary({
                            salesSummaryDatePeriod: datePeriod
                        })
                        .done(function (result) {
                            self.graph.setData(result.salesSummary);
                            self.graph.redraw();
                        });
                };

                var draw = function (data) {
                    if (!this.graph) {
                        this.graph = init(data);
                    } else {
                        this.graph.setData(data);
                        this.graph.redraw();
                    }
                };

                return {
                    draw: draw,
                    refresh: refresh,
                    graph: instance
                };
            };

            _widget.find("#salesStatistics").show();

            var randomId = getRandomChartId();
            $(_widget).find('.salesStatisticsChart').attr('id', randomId);
            var salesSummary = new SalesSummaryChart(randomId);
            salesSummary.draw(salesSummaryData);
            $(_widget).find("input[name='SalesSummaryDateInterval']").unbind('change');
            $(_widget).find("input[name='SalesSummaryDateInterval']").change(function () {
                salesSummary.refresh(this.value);
            });

            _widget.find("#totalSales").text(totalSales);
            _widget.find("#revenue").text(revenue);
            _widget.find("#expenses").text(expenses);
            _widget.find("#growth").text(growth);
            _widget.find("#salesStatisticsLoading").hide();
        };

        var getSalesSummary = function () {
            abp.ui.setBusy(_widget);
            _tenantDashboardService
                .getSalesSummary({
                    salesSummaryDatePeriod: salesSummaryDatePeriod.daily
                })
                .done(function (result) {
                    initSalesSummaryChart(result.salesSummary, result.totalSales, result.revenue, result.expenses, result.growth);
                }).always(function () {
                abp.ui.clearBusy(_widget);
            });
        };

        function getRandomChartId() {
            return 'salesStatistics' + (Math.floor((Math.random() * 1000000))) + new Date().getTime()
        }
    }
})();