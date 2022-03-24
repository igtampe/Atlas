import { Paper, Table, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import React from 'react';
import { ParseFormattedTextList } from './FormattedText';

export function ParseTable(TableText) {
    return (<>
    <TableContainer component={Paper}>
        <Table>
            <TableHead>
                { Boolean(TableText.title)
                    ? <TableRow><TableCell style={{textAlign:'center'}} colSpan={TableText.width}><b>{TableText.title}</b></TableCell></TableRow>
                    : <></> }
                { Boolean(TableText.headerRow)
                    ? ParseRow(TableText.headerRow)
                    : <></> }
            </TableHead>
            {TableText.rows.map(a=>ParseRow(a))}
        </Table>
    </TableContainer>
    </>)
}

function ParseRow(row){
    return(<TableRow> {row.cells.map(a=><TableCell>{ParseFormattedTextList(a.text)}</TableCell>)} </TableRow>)
}