<template>
  <div>
    <b-container>
        <router-link to="/">
            <h2><b-link href="/">Back</b-link></h2>
        </router-link>
        <b-card class="createForm">
            <b-form @submit="onSubmit">
                <b-form-group id="namespaceGroup"
                    label="Namespace:"
                    label-for="namespace">
                    <b-form-input id="namespace"
                        type="text"
                        v-model="model.namespace"
                        required
                        placeholder="ex. cid">
                    </b-form-input>
                </b-form-group>
                <b-form-group id="identifierGroup"
                    label="Identifier:"
                    label-for="identifier">
                    <b-form-input id="identifier"
                        type="text"
                        v-model="model.identifier"
                        required
                        placeholder="ex. 1234">
                    </b-form-input>
                </b-form-group>
                <b-button type="submit" variant="primary">Create New Identity</b-button>
            </b-form>
        </b-card>
    </b-container>
  </div>
</template>

<script>
export default {
  name: 'CreateUser',
  data() {
    return {
      model: {
        namespace: '',
        identifier: '',
      },
    };
  },
  methods: {
    onSubmit(evt) {
      evt.preventDefault();
      this.$http.post('api/idp/identity',
        {
          namespace: this.model.namespace,
          identifier: this.model.identifier,
        }).then(() => {
        this.$router.push('/');
      }).catch((err) => {
        alert(err);
      });
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
