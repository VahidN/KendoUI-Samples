(function ($) {
    $.fn.ShowChart = function (options) {
        var defaults = {
            url: '/',
            text: 'نمودار دایره ایی',
            theme: 'blueOpal',
            font: '13px bbc-nassim-bold',
            legendPosition: 'left',
            seriesField: 'Count',
            seriesCategoryField: 'Value',
            titlePosition: 'top',
            chartWidth: 400,
            chartHeight: 400,
            seriesType: 'pie',
            categoryAxisLabelsRotation: 90
        };
        options = $.extend(defaults, options);
        return this.each(function () {
            var chartDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: options.url,
                        dataType: "json",
                        contentType: 'application/json; charset=utf-8',
                        type: 'GET'
                    }
                },
                error: function (e) {
                    alert(e.errorThrown.stack);
                }
            });
            $(this).kendoChart({
                chartArea: {
                    height: options.chartHeight
                },
                theme: options.theme,
                title: {
                    text: options.text,
                    font: options.font,
                    position: options.titlePosition
                },
                legend: {
                    position: options.legendPosition,
                    labels: {
                        font: options.font
                    }
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        format: "{0}%"
                    }
                },
                dataSource: chartDataSource,
                series: [
                    {
                        type: options.seriesType,
                        field: options.seriesField,
                        categoryField: options.seriesCategoryField,
                        aggregate: "sum"
                    }
                ],
                tooltip: {
                    visible: true,
                    template: "${category}: ${value}",
                    font: options.font
                },
                categoryAxis: [{
                    labels: {
                        rotation: options.categoryAxisLabelsRotation
                    }
                }]
            });
        });
    };
})(jQuery);