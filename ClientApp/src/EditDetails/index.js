import React, { Component } from 'react';
import Grid from '@material-ui/core/Grid';
import Edit from './edit';

export default class EditDetails extends Component {
  static displayName = EditDetails.name;
  
  constructor(props) {
    super(props);
    this.state = { 
      details: [], id: this.props.id, loading: true
     }
  };
  
  componentDidMount() {
    this.populatePersonData(this.props.id);
  }

  componentDidUpdate() {
    if (this.state.id !== this.props.id)
    {
      this.setState({loadding: true, id: this.props.id});
      this.populatePersonData(this.props.id);      
    }
  }

  onChange = (e) => {
    const valueData = {[e.target.id]: e.target.value};
    const data = {...this.state.details, ...valueData };
    this.setState({details:data});
  }

  onSubmit = (e) => {
    e.preventDefault();
    // get our form data out of state

    fetch(`familytree/Override`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json'},
      body: JSON.stringify(this.state.details)
    });
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <Edit details={this.state.details} onChange={this.onChange} onSubmit={this.onSubmit} />

    return (
      <Grid container direction="column" justify="center" alignItems="center">
        <Grid item>
          {contents}
        </Grid>
      </Grid>
    );
  }

  async populatePersonData(id) {
    const response = await fetch(`familytree/OverrideDetails/${id}`);
    const data = await response.json();
    this.setState({ details: data, loading: false });
  }  

}