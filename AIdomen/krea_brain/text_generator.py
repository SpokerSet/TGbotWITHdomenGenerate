import gpt_2_simple as gpt2
from flask import Flask, jsonify, request

app = Flask(__name__)

checkpoint_dir="D:\AIdomen\checkpoint"
model_name = '124M'

@app.route('/generate_text', methods=['POST'])

def generate_text():
    input_data = request.get_json()
    input_text = input_data['input']

    sess = gpt2.start_tf_sess()
    gpt2.load_gpt2(sess,
              run_name='run1',
              checkpoint_dir=checkpoint_dir,
              model_name=model_name,
              model_dir='models',)
    
    generated_text = gpt2.generate(sess, model_name=model_name,
              temperature=0.9, include_prefix=True, prefix=f'{input_text} , www.',
              truncate='<|endoftext|>', nsamples=10, batch_size=1, length=5, return_as_list=True
              )
    return jsonify({'generate': generated_text})

if __name__ == '__main__':
    port = 5000
    app.run(host='192.168.43.109', port=port)
