using LaVida.Models;
using LaVida.Views.Cells;
using Xamarin.Forms;

namespace LaVida.Helpers
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        readonly DataTemplate  incomingDataTemplate;
        readonly DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is MessageModel messageVm))
                return null;


            return (messageVm.UserName == App.myAccount.Name)? outgoingDataTemplate: incomingDataTemplate;
        }

    }
}