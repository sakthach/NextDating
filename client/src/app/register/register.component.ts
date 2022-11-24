import { HttpClient } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  NgForm,
  ValidatorFn,
  Validators,
  AbstractControl,
} from '@angular/forms';
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

  registerForm!: FormGroup;
  url = environment.baseApiUrl;

  constructor(
    public accountService: AccountService,
    private userService: UserService,
    private route: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
      ]),
      password_again: new FormControl('', [
        Validators.required,
        this.matchValue('password'),
      ]),
    });
    // comppare passwowrd to password_again
    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['password_again'].updateValueAndValidity();
    });
  }

  // create Custom validation compare paswwor_again to passowrd.

  matchValue(matchTo: any): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control.parent?.get(matchTo).value
        ? null
        : { isMatching: true };
    };
  }

  cancle() {
    this.cancelFromRegister.emit(false);
  }
  register() {
    // const user: any = {
    //   username: this.form.value.username,
    //   password: this.form.value.password,
    // };
    // this.accountService.register(user).subscribe(() => {
    //   this.route.navigate(['/members']);
    // });
  }
}
