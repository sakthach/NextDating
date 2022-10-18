import { HttpClient } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { UserService } from 'src/services/user.service';
import { AccountService } from 'src/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelFromRegister = new EventEmitter<boolean>();
  @ViewChild('f') form!: NgForm;

  url = environment.baseApiUrl;

  constructor(
    public accountService: AccountService,
    private userService: UserService,
    private route: Router
  ) {}

  ngOnInit(): void {}

  cancle() {
    this.cancelFromRegister.emit(false);
  }
  register() {
    const user: any = {
      username: this.form.value.username,
      password: this.form.value.password,
    };

    this.accountService.register(user).subscribe(() => {
      this.route.navigate(['/members']);
    });
  }
}
