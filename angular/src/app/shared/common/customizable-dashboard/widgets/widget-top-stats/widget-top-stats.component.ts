import { Component, OnInit, Injector } from '@angular/core';
import { NameValueDto, ReportTopStat, TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { DashboardChartBase } from '../dashboard-chart-base';
import { WidgetComponentBaseComponent } from '../widget-component-base';
import { AbpSessionService } from 'abp-ng2-module';
import {
  ReportServiceProxy
} from '@shared/service-proxies/service-proxies';
import { Theme2ThemeUiSettingsComponent } from '@app/admin/ui-customization/theme2-theme-ui-settings.component';

class DashboardTopStats extends DashboardChartBase {

  total1:number = 0;
  total2:number = 0;
  total3:number = 0;
  total4:number = 0;
  total5:number = 0;
  total6:number = 0;
  total7:number = 0;  

 animationCounter1 = 0;
 animationCounter2 = 0;
 animationCounter3 = 0;
 animationCounter4 = 0;
 animationCounter5 = 0;
 animationCounter6 = 0;
 animationCounter7 = 0;

  newFeedbacksChangeCounter = 0;

  totalTaskCount:number = 0;

  personalStats: ReportTopStat[];

  init(personalStatsParam: ReportTopStat[]) {

    this.personalStats = personalStatsParam;

    var inx = 1;

    personalStatsParam.forEach(a => {
        this.totalTaskCount += a.count;

        switch (inx) {
          case 1:
            this.total1 = a.count;
            break;
            case 2:
              this.total2 = a.count;
            break;
            case 3:
              this.total3 = a.count;
            break;
            case 4:
              this.total4 = a.count;
            break;
            case 5:
              this.total5 = a.count;
            break;
            case 6:
              this.total6 = a.count;
            break;
            case 7:
              this.total7 = a.count;
            break;
        }
        inx++;
    });
    personalStatsParam.forEach(a => {
      a.percentage = parseInt(((100 * a.count) / this.totalTaskCount).toFixed(2));
  });

    //  this.totalOpenCount = parseInt(personalStatsParam[0].value);
    //  this.newFeedbacks = parseInt(personalStatsParam[1].value);
    //  this.newOrders = parseInt(personalStatsParam[2].value);
    //  this.newUsers = parseInt(personalStatsParam[3].value);

    //  this.openTaskPercentage = (100 * this.totalOpenCount) / this.totalTaskCount;     
    

    this.hideLoading();
  }

  // init(totalProfit, newFeedbacks, newOrders, newUsers) {
  //   this.totalProfit = totalProfit;
  //   this.newFeedbacks = newFeedbacks;
  //   this.newOrders = newOrders;
  //   this.newUsers = newUsers;
  //   this.hideLoading();
  // }

}

@Component({
  selector: 'app-widget-top-stats',
  templateUrl: './widget-top-stats.component.html',
  styleUrls: ['./widget-top-stats.component.css']
})
export class WidgetTopStatsComponent extends WidgetComponentBaseComponent implements OnInit {

  personalStats: ReportTopStat[] = new Array<ReportTopStat>();

  dashboardTopStats: DashboardTopStats;

  constructor(injector: Injector,
    private _tenantDashboardServiceProxy: TenantDashboardServiceProxy,
    private _reportServiceProxy: ReportServiceProxy,
    private _abpSessionService: AbpSessionService
  ) {
    super(injector);
    this.dashboardTopStats = new DashboardTopStats();
  }

  ngOnInit() {
    // this.loadTopStatsData();
    this.loadPersonalStats();
  }

  loadPersonalStats() {
  
    this._reportServiceProxy.getPersonalSummaryWidget(this._abpSessionService.userId).subscribe((data) => {
      debugger;
      this.dashboardTopStats.init(data);
    });
  }

  loadTopStatsData() {
    this._tenantDashboardServiceProxy.getTopStats().subscribe((data) => {
      // this.dashboardTopStats.init(data.totalProfit, data.newFeedbacks, data.newOrders, data.newUsers);
    });
  }
}
