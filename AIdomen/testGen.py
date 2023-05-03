import gpt_2_simple as gpt2


checkpoint_dir="D:\AIdomen\checkpoint"
model_name = '124M'


sess = gpt2.start_tf_sess()
gpt2.load_gpt2(sess,
              run_name='run1',
              checkpoint_dir=checkpoint_dir,
              model_name=model_name,
              model_dir='models',)

client_context = 'new cars and planes '

gpt2.generate(sess, model_name=model_name,
              temperature=0.9, include_prefix=True, prefix=f'{client_context} , www.',
              truncate='<|endoftext|>', nsamples=10, batch_size=1, length=5
              )

