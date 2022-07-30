﻿import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PaxTaskDetailsRoutingModule } from './create-or-edit-paxTask-routing.module';
import { CreateOrEditPaxTaskModalComponent } from './create-or-edit-paxTask.component';
import { PaxTaskUserLookupTableModalComponent } from './paxTask-user-lookup-table-modal.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import {CalendarModule} from 'primeng/calendar';
import {TimelineModule} from 'primeng/timeline';
import {CardModule} from 'primeng/card';
import {TreeSelectModule} from 'primeng/treeselect';

@NgModule({
    declarations: [
        CreateOrEditPaxTaskModalComponent,
        PaxTaskUserLookupTableModalComponent
    ],
    imports: [AppSharedModule, PaxTaskDetailsRoutingModule, AdminSharedModule, CKEditorModule, CalendarModule, TimelineModule, CardModule, TreeSelectModule],
})
export class PaxTaskDetailsModule {}
