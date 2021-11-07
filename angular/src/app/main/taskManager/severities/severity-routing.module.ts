import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SeveritiesComponent } from './severities.component';

const routes: Routes = [
    {
        path: '',
        component: SeveritiesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SeverityRoutingModule {}
