/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, ComponentFixture, ComponentFixtureAutoDetect, waitForAsync } from '@angular/core/testing';
import { BrowserModule } from '@angular/platform-browser';
import { DashboardFilterComponent } from '../app/dashboard/dashboard-filter/dashboard-filter.component';

let component: DashboardFilterComponent;
let fixture: ComponentFixture<DashboardFilterComponent>;

describe('dashboard-filter component', () => {
    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            declarations: [ DashboardFilterComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DashboardFilterComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', waitForAsync(() => {
        expect(true).toEqual(true);
    }));
});
