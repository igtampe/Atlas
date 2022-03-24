//The API URL. Either the one provided by the environment variable, or the default
export const APIURL = process.env.APIURL ?? 'https://localhost:7004';

export const GenerateJSONPost = (SessionID, Body) => {
    return({
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID },
        body: JSON.stringify(Body)
    })
}

export const GeneratePost = (SessionID, Body) => {
    return({
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID },
        body: Body
    })
}

export const GenerateJSONPut = (SessionID,Body) => {
    return({
        method: 'PUT',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID},
        body: JSON.stringify(Body)
    })
}

export const GeneratePut = (SessionID,Body) => {
    return({
        method: 'PUT',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID},
        body: Body
    })
}

export const GenerateGet = (SessionID) => {    
    return(SessionID ? {
        method: 'GET',
        headers: {'SessionID': SessionID },
    } : { method: 'GET'})
}

export const GenerateDelete = (SessionID) => {    
    return({
        method: 'DELETE',
        headers: {'SessionID': SessionID },
    })
}