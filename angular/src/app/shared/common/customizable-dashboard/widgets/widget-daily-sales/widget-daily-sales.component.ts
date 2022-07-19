import { Component, OnInit, Injector } from '@angular/core';
import { TenantDashboardServiceProxy, OrganizationUnitServiceProxy } from '@shared/service-proxies/service-proxies';
import { DashboardChartBase } from '../dashboard-chart-base';
import { WidgetComponentBaseComponent } from '../widget-component-base';
import {
  ReportServiceProxy,
  ListResultDtoOfOrganizationUnitDto
} from '@shared/service-proxies/service-proxies';
import { ArrayToTreeConverterService } from '@shared/utils/array-to-tree-converter.service';

class DailySalesLineChart extends DashboardChartBase {

  chartData: any[];
  scheme: any = {
    name: 'green',
    selectable: true,
    group: 'Ordinal',
    domain: [        
      "#eec137",
      "#4cd07d",
      "#06b6d4",
      "#54BABD",
      "#53BFBB"]
  };


  multi: any[] = [
    {
      "name": "Germany",
      "series": [
        {
          "name": "2010",
          "value": 7300000
        },
        {
          "name": "2011",
          "value": 8940000
        }
      ]
    },
  
    {
      "name": "USA",
      "series": [
        {
          "name": "2010",
          "value": 7870000
        },
        {
          "name": "2011",
          "value": 8270000
        }
      ]
    },
  
    {
      "name": "France",
      "series": [
        {
          "name": "2010",
          "value": 5000002
        },
        {
          "name": "2011",
          "value": 5800000
        }
      ]
    }
  ];;
  view: any[] = [700, 400];

  // options
  showXAxis: boolean = true;
  showYAxis: boolean = true;
  gradient: boolean = false;
  showLegend: boolean = true;
  showXAxisLabel: boolean = true;
  xAxisLabel: string = 'Country';
  showYAxisLabel: boolean = true;
  yAxisLabel: string = 'Population';
  animations: boolean = true;
  legendPosition: string  = 'right';

  colorScheme = {
    domain: ['#5AA454', '#C7B42C', '#AAAAAA']
  };
  showChart:boolean = false;

  selectedNode: any;

  nodes: any[] = 
    [
        {
            "label": "Documents",
            "data": "Documents Folder",
            "children": [{
                    "label": "Work",
                    "data": "Work Folder",
                    "children": [{"label": "Expenses.doc", "icon": "pi pi-file", "data": "Expenses Document"}, {"label": "Resume.doc", "icon": "pi pi-file", "data": "Resume Document"}]
                },
                {
                    "label": "Home",
                    "data": "Home Folder",
                    "expandedIcon": "pi pi-folder-open",
                    "collapsedIcon": "pi pi-folder",
                    "children": [{"label": "Invoices.txt", "icon": "pi pi-file", "data": "Invoices for this month"}]
                }]
        },
        {
            "label": "Pictures",
            "data": "Pictures Folder",
            "expandedIcon": "pi pi-folder-open",
            "collapsedIcon": "pi pi-folder",
            "children": [
                {"label": "barcelona.jpg", "icon": "pi pi-image", "data": "Barcelona Photo"},
                {"label": "logo.jpg", "icon": "pi pi-file", "data": "PrimeFaces Logo"},
                {"label": "primeui.png", "icon": "pi pi-image", "data": "PrimeUI Logo"}]
        },
        {
            "label": "Movies",
            "data": "Movies Folder",
            "expandedIcon": "pi pi-folder-open",
            "collapsedIcon": "pi pi-folder",
            "children": [{
                    "label": "Al Pacino",
                    "data": "Pacino Movies",
                    "children": [{"label": "Scarface", "icon": "pi pi-video", "data": "Scarface Movie"}, {"label": "Serpico", "icon": "pi pi-file-video", "data": "Serpico Movie"}]
                },
                {
                    "label": "Robert De Niro",
                    "data": "De Niro Movies",
                    "children": [{"label": "Goodfellas", "icon": "pi pi-video", "data": "Goodfellas Movie"}, {"label": "Untouchables", "icon": "pi pi-video", "data": "Untouchables Movie"}]
                }]
        }
    ];

  constructor(
    private _dashboardService: TenantDashboardServiceProxy) {
    super();
  }

  init(data) {
    this.chartData = [];
    for (let i = 0; i < data.length; i++) {
      this.chartData.push({
        name: i + 1,
        value: data[i]
      });
    }

    setTimeout(() => {this.showChart = true}, 1); 

  }
  
  onSelect(event) {
    console.log(event);
  }
  reload() {
    this.showLoading();
    this._dashboardService
      .getDailySales()
      .subscribe(result => {
        this.init(result.dailySales);
        this.hideLoading();
      });
  }
}

@Component({
  selector: 'app-widget-daily-sales',
  templateUrl: './widget-daily-sales.component.html',
  styleUrls: ['./widget-daily-sales.component.css']
})
export class WidgetDailySalesComponent extends WidgetComponentBaseComponent implements OnInit {

  dailySalesLineChart: DailySalesLineChart;

  constructor(injector: Injector,
    private _tenantdashboardService: TenantDashboardServiceProxy,
    private _reportServiceProxy: ReportServiceProxy,
    private _organizationUnitService: OrganizationUnitServiceProxy,
    private _arrayToTreeConverterService: ArrayToTreeConverterService) {
    super(injector);
    this.dailySalesLineChart = new DailySalesLineChart(this._tenantdashboardService);
  }

  ngOnInit() {
    this.dailySalesLineChart.reload();
    this.getOrganizationSchema();
  }

//   private getTreeDataFromServer(): void {
//     let self = this;
//     this._organizationUnitService.getOrganizationUnits().subscribe((result: ListResultDtoOfOrganizationUnitDto) => {
//         this.totalUnitCount = result.items.length;
//         this.treeData = this._arrayToTreeConverterService.createTree(result.items,
//             'parentId',
//             'id',
//             null,
//             'children',
//             [
//                 {
//                     target: 'label',
//                     targetFunction(item) {
//                         return item.displayName;
//                     }
//                 }, {
//                     target: 'expandedIcon',
//                     value: 'fa fa-folder-open text-warning'
//                 },
//                 {
//                     target: 'collapsedIcon',
//                     value: 'fa fa-folder text-warning'
//                 },
//                 {
//                     target: 'selectable',
//                     value: true
//                 },
//                 {
//                     target: 'memberCount',
//                     targetFunction(item) {
//                         return item.memberCount;
//                     }
//                 },
//                 {
//                     target: 'roleCount',
//                     targetFunction(item) {
//                         return item.roleCount;
//                     }
//                 }
//             ]);
//     });
// }

totalUnitCount = 0;
treeData: any;

getOrganizationSchema()
  {
    let self = this;
        this._organizationUnitService.getOrganizationUnits().subscribe((result: ListResultDtoOfOrganizationUnitDto) => {
            this.totalUnitCount = result.items.length;
            this.treeData = this._arrayToTreeConverterService.createTree(result.items,
                'parentId',
                'id',
                null,
                'children',
                [
                    {
                        target: 'label',
                        targetFunction(item) {
                            return item.displayName;
                        }
                    }, {
                        target: 'expandedIcon',
                        value: 'fa fa-folder-open text-warning'
                    },
                    {
                        target: 'collapsedIcon',
                        value: 'fa fa-folder text-warning'
                    },
                    {
                        target: 'selectable',
                        value: true
                    },
                    {
                        target: 'memberCount',
                        targetFunction(item) {
                            return item.memberCount;
                        }
                    },
                    {
                        target: 'roleCount',
                        targetFunction(item) {
                            return item.roleCount;
                        }
                    }
                ]);
        });
  }
}
