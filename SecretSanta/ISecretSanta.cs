using System.Collections.Generic;

namespace SecretSanta
{
    interface ISecretSanta 
    {
        void SendEmail(Participant secretSanta, string to);
        void RandomNameSelector();
        List<Participant> SecretSantaList(string participantsData);
    }
}
