import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { map, Observable } from 'rxjs';
import { AccountService } from 'src/_services/account.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private route: Router) {}
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user) => {
        if (user) return true;
        this.route.navigate(['/']);
        return false;
      })
    );
  }
}
