import {Component, OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";
import { FormsModule }   from '@angular/forms';

@Component({
  selector: 'app-add-new-channel',
  templateUrl: './add-new-channel.component.html',
  styleUrls: ['./add-new-channel.component.scss']
})
export class AddNewChannelComponent implements OnInit {

  constructor() {
  }

  url:string;

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {

  }

}
