using System.Collections.Generic;

namespace Pearl.UI
{
    public abstract class Filler<F> : FillerNative
    {
        #region Public Methods
        public void Fill(in IContentFiller<F> container, float time = 0)
        {
            //prendi i dati
            List<F> content = Take();
            //metti i dati
            Put(in container, ref content);

            //seleziona il dato corrente
            if (time == 0)
            {
                Select(container);
            }
            else
            {
                PearlInvoke.WaitForMethod(time, Select, container);
            }
        }
        #endregion

        #region Private Methods
        //Metti i dati nel container
        private void Put(in IContentFiller<F> container, ref List<F> content)
        {
            if (container != null)
            {
                container.FillContent(content);
            }
        }

        //Seleziona il dato corretto nel container
        private void Select(IContentFiller<F> container)
        {
            if (container != null)
            {
                F currentValue = GetCurrentValue();
                SetContent(container, currentValue);
            }
        }

        //setta i dati
        private void SetContent(IContentFiller<F> container, F currentValue)
        {
            if (container != null && currentValue != null)
            {
                container.SetContent(currentValue);
            }
        }
        #endregion

        #region Abstract 
        protected abstract F GetCurrentValue();

        //Prendi i dati
        protected abstract List<F> Take();
        #endregion
    }
}
