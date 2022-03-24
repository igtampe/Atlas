import React from 'react';

export function ParseSection(Section) {
    //Add plus 1 because H1 is actually a header and not a title oopsie
    switch (Section.level + 1) {
        case 1:
            return <h1>{Section.title}</h1>
        case 2:
            return <h2>{Section.title}</h2>
        case 3:
            return <h3>{Section.title}</h3>
        case 4:
            return <h4>{Section.title}</h4>
        case 5:
            return <h5>{Section.title}</h5>
        case 6:
            return <h6>{Section.title}</h6>
        default:
            return <>{Section.title}</>
    }
}