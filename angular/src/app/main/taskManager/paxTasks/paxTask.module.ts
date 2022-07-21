import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PaxTaskRoutingModule } from './paxTask-routing.module';
import { PaxTasksComponent } from './paxTasks.component';
import { ViewPaxTaskModalComponent } from './view-paxTask-modal.component';
import {SelectButtonModule} from 'primeng/selectbutton';

@NgModule({
    declarations: [
        PaxTasksComponent,
        ViewPaxTaskModalComponent
    ],
    imports: [AppSharedModule, PaxTaskRoutingModule, AdminSharedModule, SelectButtonModule],
})
export class PaxTaskModule {}
