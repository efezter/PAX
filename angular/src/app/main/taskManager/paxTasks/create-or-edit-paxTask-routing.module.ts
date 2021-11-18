import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateOrEditPaxTaskModalComponent } from './create-or-edit-paxTask.component';

const routes: Routes = [
    {
        path: '',
        component: CreateOrEditPaxTaskModalComponent,
        pathMatch: 'full',
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaxTaskDetailsRoutingModule {}
