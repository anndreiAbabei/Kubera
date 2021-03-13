import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
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
  private applied = false;
  public filterForm: FormGroup;
  public assets: Asset[];
  public groups: Group[];
  public collapsed = true;

  @Output()
  public filterChanged = new EventEmitter<Filter>();

  @ViewChild('dateRangePicker')
  private dtp: any;

  @ViewChild('selectAsset')
  private selectAsset: any;

  @ViewChild('selectGroup')
  private selectGroup: any;

  constructor(private readonly assetService: AssetService,
    private readonly groupService: GroupService) {
    this.filterForm = new FormGroup({
      from: new FormControl(),
      to: new FormControl(),
      asset: new FormControl(),
      group: new FormControl(),
      rangeCheck: new FormControl(),
      assetCheck: new FormControl(),
      groupCheck: new FormControl()
    });
  }

  public async ngOnInit(): Promise<void> {
    this.filterForm.get('from').disable();
    this.filterForm.get('to').disable();
    this.filterForm.get('asset').disable();
    this.filterForm.get('group').disable();

    this.assets = await this.assetService.getAll().toPromise();
    this.groups = await this.groupService.getAll().toPromise();
  }

  public performFiltering(): void {
    const filter = new Filter();

    const from = this.filterForm.get('from');
    const to = this.filterForm.get('to');
    const asset = this.filterForm.get('asset');
    const group = this.filterForm.get('group');

    if (from.enabled && to.enabled) {
      filter.from = from.value;
      filter.to = to.value;
    }

    if (asset.enabled) {
      filter.assetId = asset.value;
    }

    if (group.enabled) {
      filter.groupId = group.value;
    }

    this.filterChanged.emit(filter);

    this.applied = true;
  }

  public setStatus(): void {
    if (this.filterForm.get('rangeCheck').value) {
      const from = this.filterForm.get('from');
      const to = this.filterForm.get('to');
      const wasDisabled = from.disabled && to.disabled;

      from.enable();
      to.enable();

      if (wasDisabled) {
        this.dtp.open();
      }
    } else {
      this.filterForm.get('from').disable();
      this.filterForm.get('to').disable();
    }

    if (this.filterForm.get('assetCheck').value) {
      const asset = this.filterForm.get('asset');
      const wasDisabled = asset.disabled;

      asset.enable();

      if (wasDisabled) {
        this.selectAsset.open();
      }
    } else {
      this.filterForm.get('asset').disable();
    }

    if (this.filterForm.get('groupCheck').value) {
      const group = this.filterForm.get('group');
      const wasDisabled = group.disabled;

      group.enable();

      if (wasDisabled) {
        this.selectGroup.open();
      }
    } else {
      this.filterForm.get('group').disable();
    }
  }

  public clearFilter(): void {
    this.filterForm.get('rangeCheck').setValue(false);
    this.filterForm.get('assetCheck').setValue(false);
    this.filterForm.get('groupCheck').setValue(false);

    this.setStatus();

    if (this.applied) {
      this.performFiltering();
      this.applied = false;
    }
  }
}
