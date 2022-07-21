import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SeverityRoutingModule } from './severity-routing.module';
import { SeveritiesComponent } from './severities.component';
import { CreateOrEditSeverityModalComponent } from './create-or-edit-severity-modal.component';
import { ViewSeverityModalComponent } from './view-severity-modal.component';
import { ChangeIconModalComponent } from './change-icon-modal.component';

@NgModule({
    declarations: [SeveritiesComponent, CreateOrEditSeverityModalComponent, ViewSeverityModalComponent, ChangeIconModalComponent],
    imports: [AppSharedModule, SeverityRoutingModule, AdminSharedModule],
})
export class SeverityModule {}
