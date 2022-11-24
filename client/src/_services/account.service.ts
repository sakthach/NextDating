import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map, ReplaySubject } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  user: User = {
    username: '',
    token: '',
    photoUrl: '',
  };

  constructor(private http: HttpClient, private route: Router) {}
  baseUrl = environment.baseApiUrl;

  login(data: any) {
    return this.http.post<User>(this.baseUrl + 'acount/login', data).pipe(
      map((resp: User) => {
        if (resp) {
          localStorage.setItem('user', JSON.stringify(resp));
          this.currentUserSource.next(resp);
        }
      })
    );
  }

  register(data: any) {
    return this.http.post<User>(this.baseUrl + 'acount/register', data).pipe(
      map((resp: User) => {
        if (resp) {
          this.setCurrentUser(resp);
        }
      })
    );
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
    localStorage.setItem('user', JSON.stringify(user));
  }

  logout() {
    localStorage.removeItem('user');
    return this.currentUserSource.next(null!);
  }
  username() {
    this.currentUserSource.subscribe((resp) => {
      this.user = resp;
    });
    return this.user.username;
  }
}
