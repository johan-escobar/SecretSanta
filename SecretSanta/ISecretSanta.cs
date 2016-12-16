using System.Collections.Generic;

namespace SecretSanta
{
    interface ISecretSanta 
    {
        void SendEmail(Participant secretSanta, string to);
        void RandomNameSelector(string data);
        List<Participant> SecretSantaList();
    }
}
