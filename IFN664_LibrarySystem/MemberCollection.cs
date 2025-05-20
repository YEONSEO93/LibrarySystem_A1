using System;
using System.Linq;
using System.Collections.Generic;

namespace LibrarySystem;
    public class MemberCollection
    {
        private Member[] members = new Member[1000];
        private int count = 0;

        public bool AddMember(Member member)
        {
            if (FindMember(member.FirstName, member.LastName) != null) return false;
            if (count >= members.Length) return false;

            members[count++] = member;
            return true;
        }

        public Member? FindMember(string firstName, string lastName)
        {
            for (int i = 0; i < count; i++)
            {
                if (members[i].FirstName == firstName && members[i].LastName == lastName)
                    return members[i];
            }
            return null;
        }
        
        public bool RemoveMember(string firstName, string lastName)
        {
            for (int i = 0; i < count; i++)
            {
                if (members[i].FirstName == firstName && members[i].LastName == lastName)
                {
                    if (members[i].BorrowedMovies.Count > 0)
                        return false;

                    for (int j = i; j < count - 1; j++)
                        members[j] = members[j + 1];

                    members[count - 1] = null!;
                    count--;
                    return true;
                }
            }
            return false;
        }
        
        public Member[] GetAllMembers()
        {
            return members.Take(count).ToArray();
        }

        public List<Member> FindMembersRentingMovie(string movieTitle)
        {
            List<Member> rentingMembers = new();
            for (int i = 0; i < count; i++)
            {
                if (members[i].BorrowedMovies.Exists(m => m.Title == movieTitle))
                    rentingMembers.Add(members[i]);
            }
            return rentingMembers;
        }
    }
    
    