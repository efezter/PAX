import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaxTasksComponent } from './paxTasks.component';

const routes: Routes = [
    {
        path: '',
        component: PaxTasksComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaxTaskRoutingModule {}
