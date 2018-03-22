using Stump.Core.Reflection;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Interactives.Magus;
using Stump.Server.WorldServer.Game.Exchanges.Craft;
using Stump.Server.WorldServer.Game.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Handlers.Jobs
{
    public class JobHandler : WorldHandlerContainer
    {
        public static void SendJobDescriptionMessage(WorldClient client)
        {
            var description = Singleton<JobManager>.Instance.GetJobsDescription(client);

            client.Send(new JobDescriptionMessage(description));
        }

        public static void SendJobExperienceMultiUpdateMessage(WorldClient client)
        {
            var jobsExperiences = (from job in client.Character.Jobs
                                   let jobXpLevelFloor =
                                       Singleton<ExperienceManager>.Instance.GetJobNextLevelExperience((byte)(job.Level - 1))
                                   let jobXpNextLevelFloor = Singleton<ExperienceManager>.Instance.GetJobNextLevelExperience(job.Level)
                                   select
                                       new JobExperience((sbyte)job.Template.Id, (sbyte)job.Level, job.Experience, jobXpLevelFloor, jobXpNextLevelFloor))
                .ToList();
            client.Send(new JobExperienceMultiUpdateMessage(jobsExperiences));
        }

        public static void SendJobCrafterDirectorySettingsMessage(WorldClient client)
        {
            List<JobCrafterDirectorySettings> result = client.Character.Jobs.GetJobCrafterDirectorySettingsList();

            client.Send(new JobCrafterDirectorySettingsMessage(result));
        }

        public static void SendJobExperienceUpdateMessage(WorldClient client, Job job)
        {
            var xpLevelFloor = Singleton<ExperienceManager>.Instance.GetJobLevelExperience(job.Level);
            var xpNextFloor = Singleton<ExperienceManager>.Instance.GetJobNextLevelExperience(job.Level);

            client.Send(new JobExperienceUpdateMessage(new JobExperience((sbyte)job.Template.Id, (sbyte)job.Level, job.Experience, xpLevelFloor, xpNextFloor)));
        }

        public static void SendJobLevelUpMessage(WorldClient client, Job job)
        {
            var description = Singleton<JobManager>.Instance.GetJobsDescription(client).FirstOrDefault(x => x.jobId == job.Template.Id);
            client.Send(new JobLevelUpMessage((sbyte)job.Level, description));
        }

        [WorldHandler(6389u)]
        public static void HandleExchangeSetCraftRecipeMessage(WorldClient client, ExchangeSetCraftRecipeMessage message)
        {
            if (client.Character.Dialog is CraftDialog)
            {
                var recipe = Singleton<JobManager>.Instance.GetRecipeTemplateByResultId((short)message.objectGID);
                client.Character.CraftDialog.SetRecipe(recipe);
            }
        }


        [WorldHandler(6597u)]
        public static void HandleExchangeCraftCountRequestMessage(WorldClient client, ExchangeCraftCountRequestMessage message)
        {
            if (client.Character.Dialog is CraftDialog)
            {
                client.Character.CraftDialog.UpdateCount(message.count);
            }
            else if (client.Character.Dialog is MagusDialog)
            {
                //System.Console.WriteLine(message.count);
                ((MagusDialog)client.Character.Dialog).UpdateCount(message.count);
            }
        }

        //InteractiveUseRequestMessage
        //ExchangeStartOkRunesMessage (Runes)
        //ExhangeObjectMoved (same as always)
        //ExhangeObjectAddedMessage (same as always)
        //FocusedExchangeReadyMessage | I did all so this contains focusAction = 0 | ready = true | step = 1
        //  focusAction = it's the statId that I need from the object!
        //  step = I think it's the antibot system :p
        //ExchangeIsReadyMessage
        //Delete object/s
        //Add runes!
        //DecraftResultMessage
        //  DecraftedItemStackInfo (every deleted object!)
        //      objUid = deleted object
        //      bonusmin/max = coef, no fucking idea :) but the first three numbers are the coef :B if 2 are equals so coef the same (?)
        //      runesid = generated runes
        //      runeQty = quantity in ordered array!
        [WorldHandler(FocusedExchangeReadyMessage.Id)]
        public static void HandleFocusedExchangeReadyMessage(WorldClient client, FocusedExchangeReadyMessage message)
        {
            if (client.Character.Dialog is RunesDialog dialog)
                dialog.Execute(message.focusActionId);
        }


        [WorldHandler(ExchangeReplayStopMessage.Id)]
        public static void HandleExchangeReplayStopMessage(WorldClient client, ExchangeReplayStopMessage message)
        {
            if (client.Character.Dialog is MagusDialog)
                ((MagusDialog)client.Character.Dialog).Stop();

            using (var exchangeStoppedMessage = new ExchangeStoppedMessage(client.Character.Id))
                client.Send(exchangeStoppedMessage);

        }

        //Job Book 
        [WorldHandler(JobCrafterDirectoryListRequestMessage.Id)]
        public static void HandleJobCrafterDirectoryListRequestMessage(WorldClient client, JobCrafterDirectoryListRequestMessage message)
        {
            var list = JobBookManager.Instance.GetJobCrafterDirectoryListEntryList((byte)message.jobId);
            SendJobCrafterDirectoryListMessage(client, list);
        }

        [WorldHandler(JobBookSubscribeRequestMessage.Id)]
        public static void HandleJobBookSubscribeRequestMessage(WorldClient client, JobBookSubscribeRequestMessage message)
        {
            // on next update are an array :p 
            var sub = JobBookManager.Instance.Subscribe(message.jobIds, client.Character);
            SendJobBookSubscriptionMessage(client, sub, message.jobIds);
        }

        [WorldHandler(JobCrafterDirectoryDefineSettingsMessage.Id)]
        public static void HandleJobCrafterDirectoryDefineSettingsMessage(WorldClient client, JobCrafterDirectoryDefineSettingsMessage message)
        {
            var job = client.Character.Jobs.GetJob((byte)message.settings.jobId);
            job.FreeCraft = message.settings.free;
            job.MinLevelCraft = message.settings.minLevel;
            SendJobCrafterDirectorySettingsMessage(client);
        }

        public static void SendJobBookSubscriptionMessage(WorldClient client, bool subscribed, IEnumerable<sbyte> jobIds)
        {
            // on next update are an array :p 
            client.Send(new JobBookSubscriptionMessage(jobIds.Select(x => new JobBookSubscription(x, subscribed))));
        }

        public static void SendExchangeStartOkJobIndexMessage(WorldClient client, IEnumerable<int> index)
        {
            client.Send(new ExchangeStartOkJobIndexMessage(index));
        }

        public static void SendJobCrafterDirectoryListMessage(WorldClient client, IEnumerable<JobCrafterDirectoryListEntry> listEntries)
        {
            client.Send(new JobCrafterDirectoryListMessage(listEntries));
        }

        public static void SendExchangeStartOkRunesTradeMessage(WorldClient client)
        {
            client.Send(new ExchangeStartOkRunesTradeMessage());
        }

        public static void SendDecraftResultMessage(WorldClient client, Dictionary<int, List<Tuple<int, int>>> items)
        {
            client.Send(new DecraftResultMessage(items.Select(x => new DecraftedItemStackInfo((int)x.Key, 1, 1,
                x.Value.Select(y => (short)y.Item1), x.Value.Select(z => (int)z.Item2)))));
        }
    }
}

