import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  constructor(public accoutService: AccountService, private route: Router) {}
  user: any = {
    username: '',
    password: '',
  };
  @ViewChild('f') form!: NgForm;
  ngOnInit(): void {}

  login() {
    this.accoutService.login(this.user).subscribe((resp) => {
      this.route.navigate(['/members']);
    });
    this.form.resetForm();
  }

  logout() {
    this.accoutService.logout();
    this.route.navigate(['/']);
  }
}
