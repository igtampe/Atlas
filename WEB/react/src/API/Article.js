import { GenerateGet, GeneratePost, GeneratePut } from "./common";

export const GetArticles = (setLoading, setArticles, Query, Skip, Take) => {

    setLoading(true);

    //Build the Query
    var append = undefined;
    if (Boolean(Query)) { append = "?Query=" + Query }
    if (Boolean(Skip)) {
        if (!append) { append = "?" } else { append = "&" }
        append = append + "Skip=" + Skip
    }
    if (Boolean(Take)) {
        if (!append) { append = "?" } else { append = "&" }
        append = append + "Take=" + Take
    }

    //Fetch
    fetch(APIURL + "/API/Article" + append, GenerateGet(null))
        .then(response => { return response.json() }).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) { return; }

            setArticles(data)
            setLoading(false)

        })

}

export const GetArticle = (setLoading, Title, setArticle, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Article/" + Title, GenerateGet(Session))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setArticle(data)
            setLoading(false)

        })

}

export const UpdateArticle = (setLoading, Session, Title, ArticleText, setArticle, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Article/" + Title, GeneratePut(Session,ArticleText))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setArticle(data)
            setLoading(false)

        })

}

export const CreateArticle = (setLoading, Session, Title, ArticleText, setArticle, setError) => PostArticle(setLoading, Session, Title, ArticleText, true, setArticle, setError);
export const PreviewArticle = (setLoading, Session, Title, ArticleText, setArticle, setError) => PostArticle(setLoading, Session, Title, ArticleText, false, setArticle, setError);

const PostArticle = (setLoading, Session, Title, ArticleText, Save, setArticle, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Article/" + Title + "?Save=" + Save, GeneratePost(Session,ArticleText))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setArticle(data)
            setLoading(false)

        })

}

export const UpdateArticleEditLevel = (setLoading, Session, Title, EditLevel, setArticle, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Article/" + Title + "?NewEditLevel=" + EditLevel, GeneratePut(Session,""))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setArticle(data)
            setLoading(false)

        })

}

export const DeleteArticle = (setLoading, Session, Title, setArticle, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Article/" + Title, GenerateDelete(Session))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setArticle(data)
            setLoading(false)

        })

}
