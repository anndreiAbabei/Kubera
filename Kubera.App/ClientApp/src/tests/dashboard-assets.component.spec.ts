/// <reference path="../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, ComponentFixture, ComponentFixtureAutoDetect, waitForAsync } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { DashboardAssetsComponent } from '../app/dashboard/dashboard-assets/dashboard-assets.component';

let component: DashboardAssetsComponent;
let fixture: ComponentFixture<DashboardAssetsComponent>;

describe('dashboard-assets component', () => {
    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            declarations: [ DashboardAssetsComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(DashboardAssetsComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', waitForAsync(() => {
        expect(true).toEqual(true);
    }));
});
