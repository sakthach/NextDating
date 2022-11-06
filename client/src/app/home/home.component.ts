import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any = [];
  loggedIn = false;

  constructor(private accountSevice: AccountService, private route: Router) {}

  ngOnInit(): void {}

  cancleRegisterNow(event: boolean) {
    this.registerMode = event;
  }

  registerToggle() {
    this.accountSevice.currentUser$.subscribe((user) => {
      if (user) {
        this.route.navigate(['/']);
      } else {
        this.registerMode = !this.registerMode;
      }
    });
  }
}
