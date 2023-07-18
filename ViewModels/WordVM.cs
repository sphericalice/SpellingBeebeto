using SpellingBeebeto.Models;
using SpellingBeebeto.Utilities;

namespace SpellingBeebeto.ViewModels
{
    public class WordVM : BindableBase
    {
        private readonly Word Model;

        public string Text
        {
            get => Model.Text;
            set
            {
                Model.Text = value;
                NotifyPropertyChanged(nameof(Text));
            }
        }
        public WordVM(Word model)
        {
            Model = model;
        }
        public void DeleteLastLetter()
        {
            Model.DeleteLastLetter();
            NotifyPropertyChanged(nameof(Text));
        }
    }
}
