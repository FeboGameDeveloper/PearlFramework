using System.Collections.Generic;

public interface IContentFiller<F>
{
    // Start is called before the first frame update
    void FillContent(List<F> content);

    void SetContent(F content);
}
