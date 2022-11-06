import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { MembersService } from 'src/services/members.service';
import { Member } from 'src/_models/member';
import { User } from 'src/_models/user';
import { AccountService } from 'src/_services/account.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  member!: Member;
  user!: User;
  @ViewChild('f') myform!: NgForm;

  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.myform.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private accountService: AccountService,
    private memberService: MembersService
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user));
    this.loadMember();
  }

  ngOnInit(): void {}

  loadMember() {
    this.memberService
      .getMember(this.user.username)
      .subscribe((resp) => (this.member = resp));
  }

  updateMember() {
    this.memberService.updateMember(this.member).subscribe(() => {
      this.myform.reset(this.member);
    });
  }
}
