import { Component, OnInit, Input } from '@angular/core';
import { KeyValue } from '../Models';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {

  @Input() public keyValues: Array<KeyValue>;

  constructor() { }

  ngOnInit() {
  }

  public btnAddValueClick() {

    let isEmpty = false;

    for (let keyValue of this.keyValues) {
      if (!keyValue.value) {
        isEmpty = true;
        break;
      }
    }

    if (!isEmpty) {
      this.keyValues.push(new KeyValue(null, null));
    }
  }

  public btnDeleteValue(index: number) {
    this.keyValues.splice(index, 1);
  }

  public btnChangePosition(index: number) {
    let valueFirst = this.keyValues[0];
    this.keyValues[0] = this.keyValues[index];
    this.keyValues[index] = valueFirst;
  }
}
