import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { Asset } from 'src/models/asset.model';
import { Filter } from 'src/models/filtering.model';
import { Group } from 'src/models/group.model';
import { AssetService } from 'src/services/asset.service';
import { GroupService } from 'src/services/group.service';

@Component({
    selector: 'app-dashboard-filter',
    templateUrl: './dashboard-filter.component.html',
    styleUrls: ['./dashboard-filter.component.scss']
})
export class DashboardFilterComponent implements OnInit {
  public filterForm: FormGroup;
  public assets: Asset[];
  public groups: Group[];
  public range: boolean;
  public asset: boolean;
  public group: boolean;

  @Output()
  public filterChanged = new EventEmitter<Filter>();

  constructor(private readonly assetService: AssetService,
    private readonly groupService: GroupService) {
    this.filterForm = new FormGroup({
      from: new FormControl(),
      to: new FormControl(),
      asset: new FormControl(),
      group: new FormControl()
    });
  }

  public async ngOnInit(): Promise<void> {
    this.assets = await this.assetService.getAll().toPromise();
    this.groups = await this.groupService.getAll().toPromise();
  }

  public performFiltering(): void {
    const filter = new Filter();

    const from = this.filterForm.get('from');
    const to = this.filterForm.get('to');
    const asset = this.filterForm.get('asset');
    const group = this.filterForm.get('group');

    if (this.range) {
      filter.from = from.value;
      filter.to = to.value;
    }

    if (this.asset) {
      filter.assetId = asset.value;
    }

    if (this.group) {
      filter.groupId = group.value;
    }

    this.filterChanged.emit(filter);
  }

  public setStatus(status: boolean, ...controls: AbstractControl[]): void {
    if (controls) {
      controls.forEach(c => {
        if (status) {
          c.enable();
        } else {
          c.disable();
        }
      });
    }
  }

  public clearFilter(): void {
    this.range = false;
    this.asset = false;
    this.group = false;

    this.performFiltering();
  }
}
