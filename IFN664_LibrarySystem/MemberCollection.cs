using System;

namespace LibrarySystem
{
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

        public Member FindMember(string firstName, string lastName)
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
                    if (members[i].BorrowedCount > 0)
                        return false;

                    for (int j = i; j < count - 1; j++)
                        members[j] = members[j + 1];

                    members[count - 1] = null;
                    count--;
                    return true;
                }
            }
            return false;
        }

        public Member[] GetAllMembers()
        {
            Member[] result = new Member[count];
            for (int i = 0; i < count; i++)
                result[i] = members[i];
            return result;
        }

        public Member[] FindMembersRentingMovie(string movieTitle)
        {
            Member[] temp = new Member[count];
            int matchCount = 0;

            for (int i = 0; i < count; i++)
            {
                Movie[] borrowed = members[i].GetBorrowedMovies();
                for (int j = 0; j < borrowed.Length; j++)
                {
                    if (borrowed[j].Title == movieTitle)
                    {
                        temp[matchCount++] = members[i];
                        break;
                    }
                }
            }

            Member[] result = new Member[matchCount];
            for (int i = 0; i < matchCount; i++)
                result[i] = temp[i];
            return result;
        }
    }
}