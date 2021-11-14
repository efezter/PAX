import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PaxTaskRoutingModule } from './paxTask-routing.module';
import { PaxTasksComponent } from './paxTasks.component';
import { CreateOrEditPaxTaskModalComponent } from './create-or-edit-paxTask-modal.component';
import { ViewPaxTaskModalComponent } from './view-paxTask-modal.component';
import { PaxTaskUserLookupTableModalComponent } from './paxTask-user-lookup-table-modal.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
// import { PaxTaskUserLookupTableModalComponent } from './paxTask-user-lookup-table-modal.component';

@NgModule({
    declarations: [
        PaxTasksComponent,
        CreateOrEditPaxTaskModalComponent,
        ViewPaxTaskModalComponent,

        PaxTaskUserLookupTableModalComponent
        // PaxTaskUserLookupTableModalComponent,
    ],
    imports: [AppSharedModule, PaxTaskRoutingModule, AdminSharedModule,CKEditorModule],
})
export class PaxTaskModule {}
