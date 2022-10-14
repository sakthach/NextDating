import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { User } from '../models/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'client';
  users: User[] = [];
  constructor(private userService: UserService) {}
  ngOnInit(): void {
    this.userService.fetchUsers().subscribe((res) => {
      this.users = res;
    });
  }
}
