﻿using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Social
{
    public class Friend
    {
        public Friend(AccountRelation relation, WorldAccount account)
        {
            Relation = relation;
            Account = account;
        }

        public Friend(AccountRelation relation, WorldAccount account, Character character)
        {
            Relation = relation;
            Account = account;
            Character = character;
        }

        public WorldAccount Account
        {
            get;
        }

        public Character Character
        {
            get;
            private set;
        }

        public AccountRelation Relation
        {
            get;
        }

        public void SetOnline(Character character)
        {
            if (character.Client.WorldAccount.Id != Account.Id)
                return;

            Character = character;
        }

        public void SetOffline()
        {
            Character = null;
        }

        public bool IsOnline() => Character != null;

        public FriendInformations GetFriendInformations(Character asker)
        {
            if (IsOnline())
            {
                return new FriendOnlineInformations(Account.Id,
                    Account.Nickname,
                    (sbyte)(Character.FriendsBook.IsFriend(asker.Account.Id) ? (Character.IsFighting() ? PlayerStateEnum.GAME_TYPE_FIGHT : PlayerStateEnum.GAME_TYPE_ROLEPLAY) : PlayerStateEnum.UNKNOWN_STATE ),
                    (short)Account.LastConnectionTimeStamp,
                    0, // todo achievement
                    Character.Sex == SexTypeEnum.SEX_FEMALE,
                    false,
                    Character.Id,
                    Character.Name,
                    Character.FriendsBook.IsFriend(asker.Account.Id) ? (sbyte)Character.Level : (sbyte)0,
                    Character.FriendsBook.IsFriend(asker.Account.Id) ? (sbyte)Character.AlignmentSide : (sbyte)AlignmentSideEnum.ALIGNMENT_UNKNOWN,
                    (sbyte)Character.Breed.Id,
                    Character.GuildMember == null ? new GuildInformations(0, "", 0, new GuildEmblem(0,0,0,0)) : Character.GuildMember.Guild.GetGuildInformations(),
                    (short)Character.SmileyMoodId,
                    Character.Status);
            }

            return new FriendInformations(
                Account.Id,
                Account.Nickname,
                (sbyte) PlayerStateEnum.NOT_CONNECTED,
                (short)Account.LastConnectionTimeStamp,
                0); // todo achievement
        }
    }
}