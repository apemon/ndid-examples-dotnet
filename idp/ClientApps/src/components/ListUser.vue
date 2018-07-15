<template>
  <div>
    <b-container>
        <router-link to="/create">
            <b-button href="/" variant="primary">Create</b-button>
        </router-link>
        <b-card class="listUser" v-for="(user, index) in users" :key="index" header="Identity">
            <p>Namespace: {{ user.namespace }}</p>
            <p>Identifier: {{ user.identifier }}</p>
            <b-button @click="onClick(index)" variant="link">Requests</b-button>
        </b-card>
    </b-container>
  </div>
</template>

<script>
export default {
  name: 'ListUser',
  data() {
    return {
      users: [],
    };
  },
  methods: {
    onClick(index) {
      const user = this.users[index];
      const namespace = user.namespace;
      const identifier = user.identifier;
      this.$router.push(`/user/${namespace}/${identifier}`);
    },
  },
  mounted() {
    const url = 'api/idp/users/';
    this.$http.get(url).then((res) => {
      this.users = res.data;
    }).catch((err) => {
      alert(err);
    });
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.listUser {
    margin-bottom: 20px;
    margin-top: 20px;
}
</style>
