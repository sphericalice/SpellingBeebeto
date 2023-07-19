using SpellingBeebeto.Models.GameElements;
using SpellingBeebeto.Utilities;

namespace SpellingBeebeto.ViewModels.GameElements
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
    }
}
