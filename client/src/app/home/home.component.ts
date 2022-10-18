import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/services/user.service';
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

  constructor(
    private userService: UserService,
    private accountSevice: AccountService,
    private route: Router
  ) {}

  ngOnInit(): void {
    this.userService.fetchUsers().subscribe((res) => {
      this.users = res;
      this.accountSevice.currentUser$.subscribe((user) => {
        console.log(user);
        if (user) {
          this.loggedIn = true;
        }
      });
    });
  }

  cancleRegisterNow(event: boolean) {
    this.registerMode = event;
  }

  registerToggle() {
    this.accountSevice.currentUser$.subscribe((user) => {
      console.log(user);
      if (user) {
        this.route.navigate(['/']);
      } else {
        this.registerMode = !this.registerMode;
      }
    });
  }
}
