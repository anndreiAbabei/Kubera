import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard-create-transaction.component.html',
  styleUrls: ['./dashboard-create-transaction.component.scss']
})
export class DashboardCreateTransactionComponent implements OnInit {
  addTransactionForm: FormGroup;
  constructor(public readonly activeModal: NgbActiveModal,
    public readonly formBuilder: FormBuilder) {

    this.addTransactionForm = this.formBuilder.group({
      date: ['']
    });
  }

  ngOnInit(): void {
  }
}
