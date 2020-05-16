using Android.App;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using WorldOfEnglishWord.Adapters;
using WorldOfEnglishWord.BisnessLogic;

namespace WorldOfEnglishWord.Controllers.Dictionary
{
    class DictionaryItemTouchHelper : ItemTouchHelper.Callback
    {
        private WordsLogic wordsLogic;
        private DictionaryAdapter adapter;
        private DictionaryActivity activity;

        public DictionaryItemTouchHelper(WordsLogic wordsLogic, DictionaryAdapter adapter, DictionaryActivity activity)
        {
            this.wordsLogic = wordsLogic;
            this.adapter = adapter;
            this.activity = activity;
        }

        public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
        {
            int drag = ItemTouchHelper.Left;
            int swipe = ItemTouchHelper.Start;
            return MakeMovementFlags(drag, swipe);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(activity.Activity);
            alert.SetTitle("Подтерждение удаления");
            alert.SetMessage("Вы действительно хотите удалить это слово?");

            alert.SetPositiveButton("Удалить", (senderAlert, args) => 
            {
                wordsLogic.DeleteWordInLists(viewHolder.LayoutPosition);
                adapter.NotifyItemRemoved(viewHolder.LayoutPosition);
            });

            alert.SetNegativeButton("Отменить", (senderAlert, args) => {
                adapter.NotifyItemChanged(viewHolder.LayoutPosition);
            });

            Dialog dialog = alert.Create();
            dialog.Show();
            dialog.SetCanceledOnTouchOutside(false);
        }
    }
}