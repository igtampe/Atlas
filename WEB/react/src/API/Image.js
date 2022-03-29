import { GenerateGet, GenerateJSONPut, APIURL, GenerateDelete } from "./common";

export const GetImages = (setLoading, setImages, Query, Skip, Take) => {

    setLoading(true);

    //Build the Query
    var append = undefined;
    if (Boolean(Query)) { append = "?Query=" + Query }
    if (Boolean(Skip)) {
        if (!append) { append += "?" } else { append += "&" }
        append += append + "Skip=" + Skip
    }
    if (Boolean(Take)) {
        if (!append) { append += "?" } else { append += "&" }
        append += append + "Take=" + Take
    }

    //Fetch
    fetch(APIURL + "/API/Images" + append, GenerateGet(null))
        .then(response => { return response.json() }).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) { return; }

            setImages(data)
            setLoading(false)

        })

}

export const GetImageInfo = (setLoading, ID, setImage, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Images/" + ID + "/info", GenerateGet(null))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.reason ?? "An unknown serverside error occurred");
                return;
            }

            setImage(data)
            setLoading(false)

        })

}

export const UploadImage = (setLoading, Session, file, name, description, onSuccess, onError) => {

    setLoading(true);
    const FR = new FileReader();
    FR.addEventListener('load',(event)=>{
      
      setLoading(true)
      
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': file.type, 'Content-Length':file.size, 'SessionID': Session },
        body: event.target.result
      };
  
      fetch(APIURL + "/API/Images?Name=" + name + "&Description=" + description, requestOptions)
        .then(response => response.json() ).then(data => {

          setLoading(false)
        //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                onError(data.reason ?? "An unknown serverside error occurred");
                return;
            }

            onSuccess(data)
        })
        .catch( e => {
          setLoading(false)
          onError("Something happened")}
        )
    })

    FR.readAsArrayBuffer(file)

}

export const DeleteImage = (setLoading, Session, id, onSuccess, onError) => {

    setLoading(true);

    //Fetch
    fetch(APIURL + "/API/Images/" + id, GenerateDelete(Session))
        .then(response => { return response.json() }).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                onError(data.reason ?? "An unknown serverside error occurred");
                return;
            }

            onSuccess();
            setLoading(false)

        })

}