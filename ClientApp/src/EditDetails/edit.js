import React from 'react';
import Grid from '@material-ui/core/Grid';
import { TextField } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button'
import { Typography } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
  root: {
    '& .MuiTextField-root': {
      margin: theme.spacing(1),
      width: '25ch',
    },
  },
}));

const Edit = (props) => {
  const classes = useStyles();
  const { preferredName, fullName, birth, death, portrait, note } = props.details;

  return (
    <Grid container direction="column" justify="center" alignItems="center">
      <Grid item>
        <Typography variant="h4">
          {preferredName}
        </Typography>
      </Grid>
      <Grid item>
        <form className={classes.root} onSubmit={props.onSubmit}>
          <Grid item>
            <TextField
              id="preferredName"
              label="Preferred Name"
              defaultValue={preferredName}
              onChange={props.onChange}
            />
          </Grid>
          <Grid item>
            <TextField
              id="fullName"
              label="Full Name"
              defaultValue={fullName}
              onChange={props.onChange}
            />
          </Grid>
          <Grid item>
            <TextField
              id="birth"
              label="Birth"
              defaultValue={birth}
              onChange={props.onChange}
            />
          </Grid>
          <Grid item>
            <TextField
              id="death"
              label="Death"
              defaultValue={death}
              onChange={props.onChange}
            />
          </Grid>
          <Grid item>
            <TextField
              id="note"
              label="Note"
              defaultValue={note}
              onChange={props.onChange}
            />
          </Grid>
          <Grid item>
            <TextField
              id="portrait" 
              label="Portrait"
              defaultValue={portrait}
              onChange={props.onChange}
            />
          </Grid>
          <Grid item>
            <Button variant="contained" color="primary" type="submit">
              Submit
            </Button>
          </Grid>
        </form>
      </Grid>
    </Grid>
    );
  }

  export default Edit