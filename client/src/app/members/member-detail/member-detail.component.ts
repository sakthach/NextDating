import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MembersService } from 'src/services/members.service';
import { Member } from 'src/_models/member';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  member!: Member;
  constructor(
    private memberService: MembersService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadMember();
    console.log(this.route.snapshot.paramMap.get('userName')!);
  }
  loadMember() {
    this.memberService
      .getMember(this.route.snapshot.paramMap.get('userName')!)
      .subscribe((member) => {
        this.member = member;
      });
  }
}
