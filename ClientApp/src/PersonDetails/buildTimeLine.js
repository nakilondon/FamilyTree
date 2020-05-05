import React, { Component } from 'react';

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import { Typography, Grid } from '@material-ui/core';

export default class BuildTimeline extends Component{
    
    render() {
        if (this.props.events == null)
        {
          return <p>No Timeline</p>
        }
         
        return (
            <Grid container space={5} direction="column" justify="center" alignItems="center">
                <Grid item>
                    <Typography variant="h4">
                        Timeline
                    </Typography>
                </Grid> 
                <Grid item>
                    <TableContainer component={Paper}>
                        <Table className='table' aria-label="simple table">
                            <TableHead>
                                <TableRow> 
                                    <TableCell>Date</TableCell>
                                    <TableCell  align="right">Type</TableCell>
                                    <TableCell  align="right">Location</TableCell>        
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                { this.props.events.map(event =>
                                    <TableRow key={event.id} > 
                                        <TableCell  component="th" scope="row">{event.eventDate}</TableCell>
                                        <TableCell  align="right">{event.detail}</TableCell>
                                        <TableCell  align="right">{event.place}</TableCell>        
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </Grid>
            </Grid>
        );
    }
}